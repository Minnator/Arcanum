#region

using Arcanum.Core.CoreSystems.Nexus;
using Arcanum.Core.CoreSystems.NUI;
using Arcanum.Core.CoreSystems.Selection;
using Arcanum.Core.GameObjects.InGame.Cultural;
using Arcanum.Core.GameObjects.InGame.Map.LocationCollections;
using Arcanum.Core.GameObjects.InGame.Pops;
using Arcanum.Core.GameObjects.InGame.Religious;
using Arcanum.UI.Components.Windows.MinorWindows.PopUpEditors;

#endregion

namespace Arcanum.UI.Components.UserControls.ValueAllocators;

public sealed class MassPopPainterViewModel : ViewModelBase
{
   private const double POP_PRECISION_EPSILON = 0.001; // 0.001 = 1 Person (if 1.0 = 1k)
   private const int DECIMALS = 3;

   private static PopPainterState? _lastState = new();

   public MassPopPainterViewModel(Location[] selectedLocations)
   {
      LoadState();
      ResetFor(selectedLocations);
   }

   public void Undo()
   {
   }

   public void ApplyChanges()
   {
      var selectedLocations = Selection.GetSelectedLocations;

      // Execute Culture Transformation
      if (TargetCulture != Culture.Empty)
         ApplyAttributeTransformation(selectedLocations,
                                      PopDefinition.Field.Culture,
                                      FilterByCulture,
                                      SourceCulture,
                                      TargetCulture,
                                      CultureTransferPercent);

      // Execute Religion Transformation
      if (TargetReligion != Religion.Empty)
         ApplyAttributeTransformation(selectedLocations,
                                      PopDefinition.Field.Religion,
                                      FilterByReligion,
                                      SourceReligion,
                                      TargetReligion,
                                      ReligionTransferPercent);

      // Execute PopType Transformation
      if (TargetPopType != PopType.Empty)
         ApplyAttributeTransformation(selectedLocations,
                                      PopDefinition.Field.PopType,
                                      FilterByPopType,
                                      SourcePopType,
                                      TargetPopType,
                                      TypeTransferPercent);
   }

   public void ResetFor(Location[] selectedLocations)
   {
      HashSet<Culture> cultures = [];
      HashSet<Religion> religions = [];
      HashSet<PopType> popTypes = [];

      foreach (var location in selectedLocations)
      {
         foreach (var pop in location.Pops)
         {
            cultures.Add(pop.Culture);
            religions.Add(pop.Religion);
            popTypes.Add(pop.PopType);
         }
      }

      AvailableCultures.ClearAndAdd(cultures);
      AvailableReligions.ClearAndAdd(religions);
      AvailablePopTypes.ClearAndAdd(popTypes);

      LoadState();
   }

   private static void ApplyAttributeTransformation<T>(
      List<Location> locations,
      PopDefinition.Field field,
      bool isFilterEnabled,
      T sourceValue,
      T targetValue,
      double percentage) where T : class
   {
      var ratio = Math.Clamp(percentage / 100.0, 0.0, 1.0);

      // If ratio is basically 0 or target is invalid, skip
      if (ratio < 0.0001 || targetValue == null!)
         return;

      foreach (var loc in locations)
      {
         // Snapshot the current list to avoid concurrent modification issues
         var eligiblePops = loc.Pops
                               .Where(pop => !isFilterEnabled || EqualityComparer<T>.Default.Equals(Nx.Get<T>(pop, field), sourceValue))
                               .ToArray();

         foreach (var pop in eligiblePops)
         {
            var amountToMove = Math.Round(pop.Size * ratio, DECIMALS);

            // If the amount to move is less than 1 person, skip this pop
            if (amountToMove < POP_PRECISION_EPSILON)
               continue;

            // Resolve target identity for matching/merging
            var targetCulture = field == PopDefinition.Field.Culture ? targetValue as Culture : Nx.Get<Culture>(pop, PopDefinition.Field.Culture);
            var targetReligion = field == PopDefinition.Field.Religion ? targetValue as Religion : Nx.Get<Religion>(pop, PopDefinition.Field.Religion);
            var targetType = field == PopDefinition.Field.PopType ? targetValue as PopType : Nx.Get<PopType>(pop, PopDefinition.Field.PopType);

            // Find existing identical entry in the same location
            var existingMatch = loc.Pops.FirstOrDefault(lp =>
                                                           lp != pop &&
                                                           Nx.Get<Culture>(lp, PopDefinition.Field.Culture) == targetCulture &&
                                                           Nx.Get<Religion>(lp, PopDefinition.Field.Religion) == targetReligion &&
                                                           Nx.Get<PopType>(lp, PopDefinition.Field.PopType) == targetType);

            if (existingMatch != null)
               // OPTION A: Merge into an existing identical entry
               Nx.Set(existingMatch, PopDefinition.Field.Size, Math.Round(existingMatch.Size + amountToMove, DECIMALS));
            else if (ratio >= 0.999)
            {
               // OPTION B: 100% conversion and no existing match found
               // We just mutate the existing pop object
               Nx.Set(pop, field, targetValue);
               continue; // Move to next pop, skip the "remaining size" logic
            }
            else
            {
               // OPTION C: Partial conversion and no existing match found
               // We must clone and split
               var newPop = (PopDefinition)pop.DeepClone();
               Nx.Set(newPop, PopDefinition.Field.Size, amountToMove);
               Nx.Set(newPop, field, targetValue);
               Nx.AddToCollection(loc, Location.Field.Pops, newPop);
            }

            // --- Remainder Handling ---
            var remainingSize = Math.Round(pop.Size - amountToMove, DECIMALS);

            if (remainingSize < POP_PRECISION_EPSILON)
               Nx.RemoveFromCollection(loc, Location.Field.Pops, pop);
            else
               Nx.Set(pop, PopDefinition.Field.Size, remainingSize);
         }
      }
   }

   public void SaveState()
   {
      _lastState = new PopPainterState(FilterByCulture,
                                       SourceCulture,
                                       TargetCulture,
                                       CultureTransferPercent,
                                       FilterByReligion,
                                       SourceReligion,
                                       TargetReligion,
                                       ReligionTransferPercent,
                                       FilterByPopType,
                                       SourcePopType,
                                       TargetPopType,
                                       TypeTransferPercent);
   }

   public void LoadState()
   {
      if (_lastState == null)
         return;

      FilterByCulture = _lastState.Value.FilterByCulture;
      SourceCulture = _lastState.Value.SourceCulture;
      TargetCulture = _lastState.Value.TargetCulture;
      CultureTransferPercent = _lastState.Value.CultureTransferPercent;
      FilterByReligion = _lastState.Value.FilterByReligion;
      SourceReligion = _lastState.Value.SourceReligion;
      TargetReligion = _lastState.Value.TargetReligion;
      ReligionTransferPercent = _lastState.Value.ReligionTransferPercent;
      FilterByPopType = _lastState.Value.FilterByPopType;
      SourcePopType = _lastState.Value.SourcePopType;
      TargetPopType = _lastState.Value.TargetPopType;
      TypeTransferPercent = _lastState.Value.TypeTransferPercent;
   }

   internal record struct PopPainterState(
      bool FilterByCulture,
      Culture SourceCulture,
      Culture TargetCulture,
      double CultureTransferPercent,
      bool FilterByReligion,
      Religion SourceReligion,
      Religion TargetReligion,
      double ReligionTransferPercent,
      bool FilterByPopType,
      PopType SourcePopType,
      PopType TargetPopType,
      double TypeTransferPercent);

   #region Culture Properties

   public bool FilterByCulture
   {
      get;
      set => SetField(ref field, value);
   }

   public Culture SourceCulture
   {
      get;
      set => SetField(ref field, value);
   } = Culture.Empty;

   public Culture TargetCulture
   {
      get;
      set => SetField(ref field, value);
   } = Culture.Empty;

   public double CultureTransferPercent
   {
      get;
      set => SetField(ref field, value);
   }

   public ObservableRangeCollection<Culture> AvailableCultures { get; } = [];

   #endregion

   #region Religion Properties

   public bool FilterByReligion
   {
      get;
      set => SetField(ref field, value);
   }

   public Religion SourceReligion
   {
      get;
      set => SetField(ref field, value);
   } = Religion.Empty;

   public Religion TargetReligion
   {
      get;
      set => SetField(ref field, value);
   } = Religion.Empty;

   public double ReligionTransferPercent
   {
      get;
      set => SetField(ref field, value);
   }

   public ObservableRangeCollection<Religion> AvailableReligions { get; } = [];

   #endregion

   #region PopType Properties

   public bool FilterByPopType
   {
      get;
      set => SetField(ref field, value);
   }

   public PopType SourcePopType
   {
      get;
      set => SetField(ref field, value);
   } = PopType.Empty;

   public PopType TargetPopType
   {
      get;
      set => SetField(ref field, value);
   } = PopType.Empty;

   public double TypeTransferPercent
   {
      get;
      set => SetField(ref field, value);
   }

   public ObservableRangeCollection<PopType> AvailablePopTypes { get; } = [];

   #endregion
}