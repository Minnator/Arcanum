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

   private void LoadLastConfigButton_Click(object sender, RoutedEventArgs e)
   {
      var text = GitDataService.GetFileFromRepositoryUrl("Minnator", "Minnators-Modforge", "master", "README.md");
      
      Console.WriteLine(text);
      MessageBox.Show(text, "Modforge README", MessageBoxButton.OK, MessageBoxImage.Information);
   }
}