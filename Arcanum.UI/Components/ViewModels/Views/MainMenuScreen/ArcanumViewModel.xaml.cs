using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using Arcanum.Core.CoreSystems.IO;
using Arcanum.Core.Utils.vdfParser;
using Arcanum.UI.Components.Specific.StyleClasses;

namespace Arcanum.UI.Components.ViewModels.Views.MainMenuScreen;

public partial class ArcanumViewModel
{
   public ObservableCollection<BaseModItem> BaseMods { get; set; } = [];

   public ArcanumViewModel()
   {
      InitializeComponent();

      DataContext = this;
   }

   private void AddBaseMod(BaseModItem item) => BaseMods.Add(item);

   private void RemoveBaseMod(BaseModItem item) => BaseMods.Remove(item);

   private BaseModItem GetBaseBaseModItem()
   {
      var newItem = new BaseModItem(RemoveBaseMod) { Path = string.Empty, };

      return newItem;
   }

   private void VanillaFolderButton_Click(object sender, RoutedEventArgs e)
   {
      var defaultPath = VdfParser.GetEu5Path();
      defaultPath = defaultPath.Replace(@"\\", @"\");
      var path = IO.SelectFolder(defaultPath, "Select the EU5 vanilla folder");
      VanillaFolderTextBox.Text = path ?? string.Empty;
   }

   private void ModFolderButton_Click(object sender, RoutedEventArgs e)
   {
      var modPath = IO.SelectFolder(IO.GetUserModFolderPath, "Select mod folder");
      ModFolderTextBox.Text = modPath ?? string.Empty;
   }

   private void AddBaseModButton_Click(object sender, RoutedEventArgs e)
   {
      var newItemPath = IO.SelectFolder(IO.GetUserModFolderPath, "Select a base mod folder");
      if (string.IsNullOrEmpty(newItemPath) ||
          BaseMods.Any(item => item.Path == newItemPath) ||
          !Directory.Exists(newItemPath))
         return;

      var newItem = new BaseModItem(RemoveBaseMod) { Path = newItemPath };

      AddBaseMod(newItem);
   }
}

public class ZeroToVisibilityConverter : IValueConverter
{
   public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      if (value is int count)
         return count == 0 ? Visibility.Visible : Visibility.Collapsed;

      return Visibility.Collapsed;
   }

   public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => throw new NotImplementedException();
}