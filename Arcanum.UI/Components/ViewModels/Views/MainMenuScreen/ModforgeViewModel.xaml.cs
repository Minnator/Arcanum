using System.Windows;

namespace Arcanum.UI.Components.ViewModels.Views;

public partial class ModforgeViewModel
{
   private const string MODFORGE_URL = "https://github.com/Minnator/Minnators-Modforge";
   
   public ModforgeViewModel()
   {
      InitializeComponent();
   }

   private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
   {
      // Open link in default browser
      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
      {
         FileName = MODFORGE_URL,
         UseShellExecute = true,
      });
   }
}