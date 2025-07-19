using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Arcanum.Core.CoreSystems.IO;
using Arcanum.Core.CoreSystems.ProjectFileUtil.Mod;
using Arcanum.Core.Utils.vdfParser;
using Arcanum.UI.Components.Specific.StyleClasses;
using Arcanum.UI.Components.UserControls.MainMenuScreen;

namespace Arcanum.UI.Components.MVVM.Views.MainMenuScreen;

public partial class ArcanumViewModel
{
   public ObservableCollection<BaseModItem> BaseMods { get; set; } = [];

   private readonly MainViewModel _mainViewModel;

   public ArcanumViewModel(List<ProjectFileDescriptor> descriptors, MainViewModel mainViewModel)
   {
      _mainViewModel = mainViewModel;
      InitializeComponent();

      DataContext = this;
      VanillaFolderTextBox.Text = VdfParser.GetEu5Path();

      descriptors.Sort();

      SetRecentProjects(descriptors);
   }

   private void SetRecentProjects(List<ProjectFileDescriptor> descriptors)
   {
      for (var i = 0; i < Math.Min(4, descriptors.Count); i++)
         RecentProjectsPanel.Children.Add(new RecentProjectCard(descriptors[i], _mainViewModel));
   }

   private void AddBaseMod(BaseModItem item) => BaseMods.Add(item);

   private void RemoveBaseMod(BaseModItem item) => BaseMods.Remove(item);

   private void VanillaFolderButton_Click(object sender, RoutedEventArgs e)
   {
      var defaultPath = VdfParser.GetEu5Path();
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

   private BaseModItem? _draggedItem;

   private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
   {
      _draggedItem = GetListBoxItemUnderMouse(BaseModsListBox, e.GetPosition(BaseModsListBox));
   }

   private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
   {
      if (_draggedItem != null && e.LeftButton == MouseButtonState.Pressed)
         DragDrop.DoDragDrop(BaseModsListBox, _draggedItem, DragDropEffects.Move);
   }

   private void ListBox_Drop(object sender, DragEventArgs e)
   {
      if (e.Data.GetDataPresent(typeof(BaseModItem)))
      {
         var droppedData = e.Data.GetData(typeof(BaseModItem));
         var target = GetListBoxItemUnderMouse(BaseModsListBox, e.GetPosition(BaseModsListBox));
         if (droppedData != null && !ReferenceEquals(droppedData, target))
            if (BaseModsListBox.ItemsSource is IList list)
            {
               var newIndex = list.IndexOf(target);

               list.Remove(droppedData);
               list.Insert(newIndex, droppedData);
            }
      }

      _draggedItem = null;
   }

   private BaseModItem GetListBoxItemUnderMouse(ListBox listBox, Point position)
   {
      var element = listBox.InputHitTest(position) as DependencyObject;
      while (element != null && element is not ListBoxItem)
         element = VisualTreeHelper.GetParent(element);
      return (BaseModItem)(element as ListBoxItem)?.DataContext!;
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