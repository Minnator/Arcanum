using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Arcanum.UI.Components.Base.StyleClasses;

namespace Arcanum.UI.Components.ViewModels;

public class PropertyGridViewModel
{
   public ObservableCollection<PropertyItem> Properties { get; }

   public PropertyGridViewModel(object target)
   {
      Properties = new(GetProperties(target));
   }

   private static IEnumerable<PropertyItem> GetProperties(object target)
   {
      var type = target.GetType();
      var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

      return
      [
         .. props.Select(p => new PropertyItem
         {
            Name = p.Name,
            Category = p.GetCustomAttribute<CategoryAttribute>()?.Category ?? "Misc",
            Type = p.PropertyType,
            Value = p.GetValue(target)!,
            IsReadOnly = !p.CanWrite,
         }),
      ];
   }
}