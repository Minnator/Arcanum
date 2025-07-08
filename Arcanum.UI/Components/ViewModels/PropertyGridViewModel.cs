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
         .. props
           .Where(p => p.CanRead)
           .Select(p =>
            {
               var getter = () => p.GetValue(target)!;
               Action<object>? setter = p.CanWrite ? v => p.SetValue(target, v) : null;

               return new PropertyItem(name: p.Name,
                                       type: p.PropertyType,
                                       getter: getter,
                                       setter: setter,
                                       category: p.GetCustomAttribute<CategoryAttribute>()?.Category ?? "Misc");
            }),
      ];
   }
}