using Arcanum.UI.Components.ViewModels.Views;
using Arcanum.UI.Components.ViewModels.Views.MainMenuScreen;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arcanum.UI.Components.ViewModels;

public class MainViewModel : ObservableObject
{
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
   public AttributionsViewModel AttributionsesVm { get; set; }
   private object _currentView = null!;

   public object CurrentView
   {
      get => _currentView;
      set
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
      ArcanumVm = new();
      AboutUsVm = new();
      AttributionsesVm = new();

      CurrentView = HomeVm;

      HomeVc = new(() => { CurrentView = HomeVm; });
      FeatureVc = new(() => { CurrentView = FeatureFm; });
      ModforgeVc = new(() => { CurrentView = ModforgeVm; });
      ArcanumVc = new(() => { CurrentView = ModforgeVm; });
      AboutUsVc = new(() => { CurrentView = AboutUsVm; });
      AttributionsVc = new(() => { CurrentView = AttributionsesVm; });
   }
}