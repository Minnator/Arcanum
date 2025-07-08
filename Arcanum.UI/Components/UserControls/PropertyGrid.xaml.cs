using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Arcanum.UI.Components.Base.StyleClasses;
using Arcanum.UI.Components.Windows;
using Arcanum.UI.Components.Windows.PopUp;

namespace Arcanum.UI.Components.UserControls;

public partial class PropertyGrid
{
   public PropertyGrid()
   {
      InitializeComponent();
      Properties = [];
      BorderThickness = new(2);
      Margin = new(2);
      PropertyList.SelectionChanged += OnPropertyListOnSelectionChanged;
   }

   private void OnPropertyListOnSelectionChanged(object sender, SelectionChangedEventArgs _)
   {
      if (sender is not ListBox { SelectedItem: PropertyItem item })
      {
         Description = "";
         return;
      }

      if (SelectedObject == null)
         return;

      var prop = SelectedObject.GetType().GetProperty(item.Name);
      var attr = prop?.GetCustomAttribute<DescriptionAttribute>();
      Description = attr?.Description ?? "No description.";
   }

   public double LabelWidth { get; set; } = 150;

   public static readonly DependencyProperty SelectedObjectProperty =
      DependencyProperty.Register(nameof(SelectedObject),
                                  typeof(object),
                                  typeof(PropertyGrid),
                                  new(null, OnSelectedObjectChanged));

   public static readonly DependencyProperty TitleProperty =
      DependencyProperty.Register(nameof(Title),
                                  typeof(string),
                                  typeof(PropertyGrid),
                                  new("Property Grid"));

   public static readonly DependencyProperty DescriptionProperty =
      DependencyProperty.Register(nameof(Description),
                                  typeof(string),
                                  typeof(PropertyGrid),
                                  new(""));

   public string Description
   {
      get => (string)GetValue(DescriptionProperty);
      set => SetValue(DescriptionProperty, value);
   }

   public object? SelectedObject
   {
      get => GetValue(SelectedObjectProperty);
      set => SetValue(SelectedObjectProperty, value);
   }

   // Returns the ToString representation of the selected object
   public string Title => SelectedObject?.ToString() ?? "Property Grid";

   public ObservableCollection<PropertyItem> Properties { get; }

   private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      if (d is not PropertyGrid grid)
         return;

      grid.Properties.Clear();

      if (e.NewValue == null)
         return;

      var props = e.NewValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
      foreach (var prop in props)
      {
         if (!prop.CanRead)
            continue;

         var categoryAttr = prop.GetCustomAttribute<CategoryAttribute>();
         var target = e.NewValue;
         Action<object>? setter = prop.CanWrite
                                     ? v =>
                                     {
                                        var targetType = prop.PropertyType;
                                        var safeValue = v == null! || targetType.IsInstanceOfType(v)
                                                           ? v
                                                           : Convert.ChangeType(v,
                                                                                targetType,
                                                                                CultureInfo.InvariantCulture);
                                        prop.SetValue(target, safeValue);
                                     }
                                     : null;

         grid.Properties.Add(new(prop.Name, prop.PropertyType, Getter, setter, categoryAttr?.Category!));
         continue;

         object Getter() => prop.GetValue(target)!;
      }
   }

   private void ViewCollection_Button_Click(object sender, RoutedEventArgs e)
   {
      if (sender is not BaseButton { DataContext: PropertyItem item })
         return;

      var collection = item.Value as ICollection;
      if (collection == null)
         return;

      var collectionView = new BaseCollectionView(collection);
      collectionView.ShowDialog();
   }

   private void ViewObject_Button_Click(object? sender, RoutedEventArgs e)
   {
      if (sender is not BaseButton { DataContext: PropertyItem item })
         return;

      if (item.Value == null!)
         return;

      var objectView = new PropertyGridWindow(item.Value);
      objectView.ShowDialog();
   }
}