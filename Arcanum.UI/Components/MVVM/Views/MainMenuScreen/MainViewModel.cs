﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Arcanum.Core.CoreSystems.ProjectFileUtil.Mod;
using Arcanum.Core.Globals;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Arcanum.UI.Components.Windows.MainWindows.MainMenuScreen;

namespace Arcanum.UI.Components.MVVM.Views.MainMenuScreen;

public class MainViewModel : ObservableObject
{
   internal MainMenuScreenView TargetedView { get; set; } = MainMenuScreenView.Home;

   internal StackPanel MainMenuStackPanel { get; set; } = null!;

   public RelayCommand HomeVc { get; set; }
   public RelayCommand ModforgeVc { get; set; }
   public RelayCommand FeatureVc { get; set; }
   public RelayCommand ArcanumVc { get; set; }
   public RelayCommand AboutUsVc { get; set; }
   public RelayCommand AttributionsVc { get; set; }

   public HomeViewModel HomeVm { get; set; }
   public ModforgeViewModel ModforgeVm { get; set; }
   public FeatureViewModel FeatureFm { get; set; }
   public ArcanumViewModel ArcanumVm { get; set; }
   public AboutUsViewModel AboutUsVm { get; set; }
   public AttributionsViewModel AttributionsVm { get; set; }
   private object _currentView = null!;
   private Visibility _isWindowVisible = Visibility.Visible;

   public Visibility IsWindowVisible
   {
      get => _isWindowVisible;
      set
      {
         if (value == _isWindowVisible)
            return;

         _isWindowVisible = value;
         OnPropertyChanged();
      }
   }

   public object CurrentView
   {
      get => _currentView;
      private set
      {
         _currentView = value;
         OnPropertyChanged();
      }
   }

   public MainViewModel()
   {
      HomeVm = new();
      ModforgeVm = new();
      FeatureFm = new();
      ArcanumVm = new(AppData.MainMenuScreenDescriptor.ProjectFiles, this);
      AboutUsVm = new();
      AttributionsVm = new();

      CurrentView = HomeVm;

      HomeVc = new(() => { SetCurrentView(MainMenuScreenView.Home); });
      FeatureVc = new(() => { SetCurrentView(MainMenuScreenView.Feature); });
      ModforgeVc = new(() => { SetCurrentView(MainMenuScreenView.Modforge); });
      ArcanumVc = new(() => { SetCurrentView(MainMenuScreenView.Arcanum); });
      AboutUsVc = new(() => { SetCurrentView(MainMenuScreenView.AboutUs); });
      AttributionsVc = new(() => { SetCurrentView(MainMenuScreenView.Attributions); });
   }

   internal void SetCurrentView(MainMenuScreenView view)
   {
      Debug.Assert(MainMenuStackPanel != null, "MainMenuStackPanel should not be null");

      switch (view)
      {
         case MainMenuScreenView.Home:
            CurrentView = HomeVm;
            break;
         case MainMenuScreenView.Modforge:
            CurrentView = ModforgeVm;
            break;
         case MainMenuScreenView.Feature:
            CurrentView = FeatureFm;
            break;
         case MainMenuScreenView.Arcanum:
            CurrentView = ArcanumVm;
            break;
         case MainMenuScreenView.AboutUs:
            CurrentView = AboutUsVm;
            break;
         case MainMenuScreenView.Attributions:
            CurrentView = AttributionsVm;
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(view), view, null);
      }

      // Update the button state of the MainMenuStackPanel
      var buttons = MainMenuStackPanel.Children.OfType<RadioButton>().ToList();
      buttons.ForEach(button => button.IsChecked = false);
      
      if (MainMenuStackPanel.Children.Count > (int)view)
         ((RadioButton)MainMenuStackPanel.Children[(int)view]).IsChecked = true;

      TargetedView = view;
   }

   internal bool GetDescriptorFromInput(out ProjectFileDescriptor descriptor)
   {
      descriptor = new(Path.GetFileName(ArcanumVm.ModFolderTextBox.Text.TrimEnd(Path.DirectorySeparatorChar)),
                       ArcanumVm.ModFolderTextBox.Text,
                       ArcanumVm.BaseMods.Select(mod => mod.Path).ToList());

      return descriptor.IsValid();
   }

   // This is the main entry point for the Arcanum application from the main menu.
   // When creating a new project, this method will be called.
   // It validates the project file and launches into the main window of Arcanum
   // if all requirements are met.
   internal async Task LaunchArcanum(ProjectFileDescriptor descriptor)
   {
      if (!descriptor.IsValid() || !Directory.Exists(ArcanumVm.VanillaFolderTextBox.Text))
      {
         MessageBox.Show("Could not create a valid 'ProjectDescriptor'.\n" +
                         "Please make sure to have valid paths for the mod- and the vanilla folder.\n\n " +
                         "If you are using base mods make sure that they are valid, too.",
                         "Invalid Project Data",
                         MessageBoxButton.OK,
                         MessageBoxImage.Error);
         return;
      }

      // Save the paths to the MainMenuScreenDescriptor
      AppData.MainMenuScreenDescriptor.LastVanillaPath =
         ArcanumVm.VanillaFolderTextBox.Text;
      AppData.MainMenuScreenDescriptor.LastProjectFile = descriptor.ModName;

      if (AppData.MainMenuScreenDescriptor.ProjectFiles
                 .Any(x => x.ModName.Equals(descriptor.ModName, StringComparison.OrdinalIgnoreCase)))
      {
         AppData.MainMenuScreenDescriptor.ProjectFiles.RemoveAll(x => x.ModName.Equals(descriptor.ModName,
                                                                     StringComparison.OrdinalIgnoreCase));
      }

      AppData.MainMenuScreenDescriptor.ProjectFiles.Add(descriptor);

      IsWindowVisible = Visibility.Collapsed;
      var loadingScreen = new Windows.MainWindows.LoadingScreen();
      await loadingScreen.ShowLoadingAsync();
      var mw = new Windows.MainWindows.MainWindow();
      mw.ShowDialog();
      GC.Collect();
      GC.WaitForPendingFinalizers();
      IsWindowVisible = Visibility.Visible;
   }
}