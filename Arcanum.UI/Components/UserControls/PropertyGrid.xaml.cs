using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Arcanum.UI.Components.Base.StyleClasses;

namespace Arcanum.UI.Components.UserControls;

public partial class PropertyGrid : UserControl
{
   public PropertyGrid()
   {
      InitializeComponent();
      Properties = [];
   }

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
         grid.Properties.Add(new()
         {
            Name = prop.Name,
            Category = categoryAttr?.Category ?? "Misc",
            Type = prop.PropertyType,
            Value = prop.GetValue(e.NewValue)!,
            IsReadOnly = !prop.CanWrite,
         });
      }
   }
}