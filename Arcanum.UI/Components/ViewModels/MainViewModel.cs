using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arcanum.UI.Components.ViewModels;

public class MainViewModel : ObservableObject
{
   public RelayCommand HomeVc { get; set; }
   public RelayCommand ModforgeVc { get; set; }
   public RelayCommand DiscoveryVc { get; set; }

   public HomeViewModel HomeVm { get; set; }
   public DiscoveryViewModel DiscoveryVm { get; set; }
   public ModforgeViewModel ModforgeVm { get; set; }
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
      DiscoveryVm = new();
      ModforgeVm = new();
      CurrentView = HomeVm;

      HomeVc = new(() => { CurrentView = HomeVm; });
      DiscoveryVc = new(() => { CurrentView = DiscoveryVm; });
      ModforgeVc = new(() => { CurrentView = ModforgeVm; });
   }
}