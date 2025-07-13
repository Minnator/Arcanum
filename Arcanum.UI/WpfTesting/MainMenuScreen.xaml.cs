using System.Windows;
using Arcanum.Core.Utils.Git;
using Arcanum.UI.Components.ViewModels;

namespace Arcanum.UI.WpfTesting;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainMenuScreen
{
   public MainMenuScreen()
   {
      InitializeComponent();
   }

   private void CloseButton_Click(object sender, RoutedEventArgs e)
   {
      Close();
   }

   private void CreateNewProjectButton_Click(object sender, RoutedEventArgs e)
   {
      var mainViewModel = (MainViewModel)DataContext;
      ArcanumTabButton.IsChecked = true;
      mainViewModel.ArcanumVc.Execute(null);
   }

   private async void LoadLastConfigButton_Click(object sender, RoutedEventArgs e)
   {
      // var (name, version) = await ArcanumGit.GetLatestReleaseNameAndVersion();
      // MessageBox.Show($"Latest release: {name} (Version: {version})", "Latest Release", MessageBoxButton.OK, MessageBoxImage.Information);
   }
}