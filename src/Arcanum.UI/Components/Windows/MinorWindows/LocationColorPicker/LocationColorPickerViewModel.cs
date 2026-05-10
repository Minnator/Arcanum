#region

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Arcanum.Core.CoreSystems.IO;
using Arcanum.Core.CoreSystems.Nexus;
using Arcanum.Core.CoreSystems.Parsing.ParsingMaster;
using Arcanum.Core.CoreSystems.Parsing.Steps.InGame.Map;
using Arcanum.Core.CoreSystems.Queastor;
using Arcanum.Core.GameObjects.InGame.Map;
using Arcanum.Core.GameObjects.InGame.Map.LocationCollections;
using Arcanum.Core.GlobalStates;
using Arcanum.Core.Utils.Colors;
using Arcanum.UI.Components.Windows.DebugWindows;
using Arcanum.UI.Components.Windows.PopUp;
using Common.UI.MBox;

#endregion

namespace Arcanum.UI.Components.Windows.MinorWindows.LocationColorPicker;

public sealed class LocationColorPickerViewModel : INotifyPropertyChanged
{
   private readonly Random _random = new();

   public LocationColorPickerViewModel()
   {
      SuggestUnusedCommand = new RelayCommand(_ => SuggestUnused());
      CreateShadeCommand = new RelayCommand(_ => CreateShade());
      ConfirmCommand = new RelayCommand(o => Confirm((o as Window)!));
      CopyRgbCommand = new RelayCommand(_ => Clipboard.SetText(RgbText));
      CopyHexCommand = new RelayCommand(_ => Clipboard.SetText(HexText));

      _usedColors = Globals.Locations.Values.Select(x => x.Color.AsInt()).ToHashSet();
      (DescriptorDefinitions.LocationDescriptor.LoadingService[0] as LocationFileLoading)!.AddPlaceholders(_usedColors);
   }

   public Color SelectedColor
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
         OnPropertyChanged(nameof(ColorBrush));
         OnPropertyChanged(nameof(RgbText));
         OnPropertyChanged(nameof(HexText));
      }
   } = Color.FromRgb(22, 87, 190);

   public Color ReferenceColor
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
         OnPropertyChanged(nameof(ReferenceBrush));
         OnPropertyChanged(nameof(ReferenceRgbText));
         OnPropertyChanged(nameof(ReferenceHexText));
      }
   } = Color.FromRgb(22, 87, 190);

   // Dropdown Selections
   public Province TargetProvince
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
      }
   } = Province.Empty;
   public Climate TargetClimate
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
      }
   } = Climate.Empty;
   public Topography TargetTopography
   {
      get;
      set
      {
         field = value;
         OnPropertyChanged();
      }
   } = Topography.Empty;

   public SolidColorBrush ColorBrush => new(SelectedColor);
   public SolidColorBrush ReferenceBrush => new(ReferenceColor);
   public string RgbText => $"{SelectedColor.R} {SelectedColor.G} {SelectedColor.B}";
   public string HexText => $"#{SelectedColor.R:X2}{SelectedColor.G:X2}{SelectedColor.B:X2}";
   public string ReferenceRgbText => $"{ReferenceColor.R} {ReferenceColor.G} {ReferenceColor.B}";
   public string ReferenceHexText => $"#{ReferenceColor.R:X2}{ReferenceColor.G:X2}{ReferenceColor.B:X2}";

   public ICommand SuggestUnusedCommand { get; }
   public ICommand CreateShadeCommand { get; }
   public ICommand ConfirmCommand { get; }
   public ICommand CopyRgbCommand { get; }
   public ICommand CopyHexCommand { get; }

   public event PropertyChangedEventHandler? PropertyChanged;

   private readonly HashSet<int> _usedColors;

   private void SuggestUnused()
   {
      SelectedColor = ColorGenerator.GenerateUnusedColor(_usedColors, true);
   }

   private void CreateShade()
   {
      SelectedColor = ColorGenerator.GenerateUnusedShade(_usedColors, ReferenceColor);
   }

   private void Confirm(Window window)
   {
      var err = string.Empty;
      if (TargetProvince == Province.Empty)
         err += "No Province selected.\n";
      if (TargetClimate == Climate.Empty)
         err += "No Climate selected.\n";
      if (TargetTopography == Topography.Empty)
         err += "No Topography selected.\n";

      if (!string.IsNullOrEmpty(err))
      {
         MBox.Show(err, "Invalid Input", MBoxButton.OK, MessageBoxImage.Error);
         return;
      }

      var template = new LocationTemplateData();
      Nx.Set(template, LocationTemplateData.Field.Climate, TargetClimate);
      Nx.Set(template, LocationTemplateData.Field.Topography, TargetTopography);

      var newLocation = Eu5ObjectCreator.ShowOnlyNamePickingPopUp(typeof(Location), _ => { });

      if (newLocation == null)
         return;

      if (string.IsNullOrEmpty(newLocation.UniqueId))
      {
         MBox.Show("Invalid Location name.", "Error", MBoxButton.OK, MessageBoxImage.Error);
         return;
      }

      Nx.Set(newLocation, Location.Field.TemplateData, template);
      Nx.AddToCollection(TargetProvince, Province.Field.Locations, newLocation);
      ((Location)newLocation).ColorIndex = LocationFileLoading.ColorIndex++;

      template.UniqueId = newLocation.UniqueId;
      Globals.LocationTemplateData.Add(template.UniqueId, template);

      Queastor.GlobalInstance.AddToIndex(template);

      AppendDefinition((Location)newLocation);
   }

   private void AppendDefinition(Location newLocation)
   {
      var files = DescriptorDefinitions.LocationDescriptor.Files;
      if (files.Count == 0)
         throw new InvalidOperationException("No location files found to append to.");

      var fileObj = files[^1];

      var oldPath = fileObj.GetFullPath();
      var moved = false;
      if (!fileObj.IsModded && Config.Settings.SavingConfig.MoveFilesToModdedDataSpaceOnSaving)
      {
         fileObj.Path.MoveToMod();
         moved = true;
      }

      fileObj.Path.UnregisterWatcher();
      if (moved)
         IO.CopyTo(oldPath, fileObj.GetFullPath());
      IO.WriteAllTextUtf8WithBom(fileObj.GetFullPath(), $"\n{newLocation.UniqueId} = {newLocation.Color.AsHex().ToString().ToLower()}", true);
      fileObj.Path.RegisterWatcher();
   }

   private void OnPropertyChanged([CallerMemberName] string name = null!) => PropertyChanged?.Invoke(this, new(name));
}