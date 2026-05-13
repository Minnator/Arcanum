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

   // Toggles for active transformation
   public bool ModifyCulture
   {
      get;
      set
      {
         if (value == field)
            return;

         field = value;
         OnPropertyChanged();
      }
   } = true;
   public bool ModifyReligion
   {
      get;
      set
      {
         if (value == field)
            return;

         field = value;
         OnPropertyChanged();
      }
   } = true;
   public bool ModifyPopType
   {
      get;
      set
      {
         if (value == field)
            return;

         field = value;
         OnPropertyChanged();
      }
   } = true;

   public double GlobalTransferPercent
   {
      get;
      set
      {
         if (SetField(ref field, value))
            RefreshPreview(Selection.GetSelectedLocations.ToArray());
      }
   } = 100;

   // Preview Properties
   public int AffectedPopsCount { get; private set; }
   public int AffectedLocationsCount { get; private set; }

   public void Undo()
   {
   }

   public void ApplyChanges()
   {
      var selectedLocations = Selection.GetSelectedLocations;
      ApplyCombinedTransformation(selectedLocations.ToList());

      // Save state after a successful apply
      SaveState();
   }

   private void ApplyCombinedTransformation(List<Location> locations)
   {
      var ratio = Math.Clamp(GlobalTransferPercent / 100.0, 0.0, 1.0);

      if (ratio < 0.0001)
         return;

      // We have nothing to set.
      var hasTarget = (ModifyCulture && TargetCulture != Culture.Empty) ||
                      (ModifyReligion && TargetReligion != Religion.Empty) ||
                      (ModifyPopType && TargetPopType != PopType.Empty);

      if (!hasTarget)
         return;

      foreach (var loc in locations)
      {
         var eligiblePops = GetEligiblePops(loc);

         foreach (var pop in eligiblePops)
         {
            var amountToMove = Math.Round(pop.Size * ratio, DECIMALS);
            if (amountToMove < POP_PRECISION_EPSILON)
               continue;

            var targetCulture = ModifyCulture && TargetCulture != Culture.Empty ? TargetCulture : pop.Culture;
            var targetReligion = ModifyReligion && TargetReligion != Religion.Empty ? TargetReligion : pop.Religion;
            var targetType = ModifyPopType && TargetPopType != PopType.Empty ? TargetPopType : pop.PopType;

            // 3. Find existing identical entry for the NEW identity
            var existingMatch = loc.Pops.FirstOrDefault(lp =>
                                                           lp != pop &&
                                                           Nx.Get<Culture>(lp, PopDefinition.Field.Culture) == targetCulture &&
                                                           Nx.Get<Religion>(lp, PopDefinition.Field.Religion) == targetReligion &&
                                                           Nx.Get<PopType>(lp, PopDefinition.Field.PopType) == targetType);

            if (existingMatch != null)
               // OPTION A: Merge into existing
               Nx.Set(existingMatch, PopDefinition.Field.Size, Math.Round(existingMatch.Size + amountToMove, DECIMALS));
            else if (ratio >= 0.999)
            {
               // OPTION B: 100% conversion - mutate the pop directly
               Nx.Set(pop, PopDefinition.Field.Culture, targetCulture);
               Nx.Set(pop, PopDefinition.Field.Religion, targetReligion);
               Nx.Set(pop, PopDefinition.Field.PopType, targetType);
               continue;
            }
            else
            {
               // OPTION C: Create a new slice
               var newPop = (PopDefinition)pop.DeepClone();
               Nx.Set(newPop, PopDefinition.Field.Size, amountToMove);
               Nx.Set(newPop, PopDefinition.Field.Culture, targetCulture);
               Nx.Set(newPop, PopDefinition.Field.Religion, targetReligion);
               Nx.Set(newPop, PopDefinition.Field.PopType, targetType);
               Nx.AddToCollection(loc, Location.Field.Pops, newPop);
            }

            // 4. Handle Remainder
            var remainingSize = Math.Round(pop.Size - amountToMove, DECIMALS);
            if (remainingSize < POP_PRECISION_EPSILON)
               Nx.RemoveFromCollection(loc, Location.Field.Pops, pop);
            else
               Nx.Set(pop, PopDefinition.Field.Size, remainingSize);
         }
      }
   }

   private PopDefinition[] GetEligiblePops(Location loc)
   {
      return loc.Pops.Where(pop =>
                 {
                    var cMatch = !ModifyCulture || SourceCulture == Culture.Empty || pop.Culture == SourceCulture;
                    var rMatch = !ModifyReligion || SourceReligion == Religion.Empty || pop.Religion == SourceReligion;
                    var tMatch = !ModifyPopType || SourcePopType == PopType.Empty || pop.PopType == SourcePopType;
                    return cMatch && rMatch && tMatch;
                 })
                .ToArray();
   }

   public void OnRequestRefreshPreview()
   {
      RefreshPreview(Selection.GetSelectedLocations.ToArray());
   }

   private void RefreshPreview(Location[] locations)
   {
      var totalPops = 0;
      var totalLocs = 0;

      foreach (var loc in locations)
      {
         var matches = GetEligiblePops(loc);

         if (matches.Any())
         {
            totalPops += matches.Length;
            totalLocs++;
         }
      }

      AffectedPopsCount = totalPops;
      AffectedLocationsCount = totalLocs;
      OnPropertyChanged(nameof(AffectedPopsCount));
      OnPropertyChanged(nameof(AffectedLocationsCount));
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

      RefreshPreview(selectedLocations);

      LoadState();
   }

   public void SaveState()
   {
      _lastState = new PopPainterState(ModifyCulture,
                                       SourceCulture,
                                       TargetCulture,
                                       ModifyReligion,
                                       SourceReligion,
                                       TargetReligion,
                                       ModifyPopType,
                                       SourcePopType,
                                       TargetPopType,
                                       GlobalTransferPercent);
   }

   public void LoadState()
   {
      if (_lastState == null)
         return;

      var state = _lastState.Value;

      ModifyCulture = state.ModifyCulture;
      SourceCulture = state.SourceCulture;
      TargetCulture = state.TargetCulture;

      ModifyReligion = state.ModifyReligion;
      SourceReligion = state.SourceReligion;
      TargetReligion = state.TargetReligion;

      ModifyPopType = state.ModifyPopType;
      SourcePopType = state.SourcePopType;
      TargetPopType = state.TargetPopType;

      GlobalTransferPercent = state.GlobalTransferPercent;
   }

   private record struct PopPainterState(
      bool ModifyCulture,
      Culture SourceCulture,
      Culture TargetCulture,
      bool ModifyReligion,
      Religion SourceReligion,
      Religion TargetReligion,
      bool ModifyPopType,
      PopType SourcePopType,
      PopType TargetPopType,
      double GlobalTransferPercent);

   #region Culture Properties

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