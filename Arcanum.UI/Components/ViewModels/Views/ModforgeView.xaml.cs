using System.Windows;

namespace Arcanum.UI.Components.ViewModels.Views;

public partial class ModforgeView
{
   private const string MODFORGE_URL = "https://github.com/Minnator/Minnators-Modforge";
   
   public ModforgeView()
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