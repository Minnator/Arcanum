using System.Windows;
using System.Windows.Controls;
using Arcanum.Core.Utils.DelayedEvents;

namespace Arcanum.UI.Components.UserControls;

public class PropertyEditorTemplateSelector : DataTemplateSelector
{
   public DataTemplate StringTemplate { get; set; } = null!;
   public DataTemplate BoolTemplate { get; set; } = null!;
   public DataTemplate EnumTemplate { get; set; } = null!;
   public DataTemplate IntTemplate { get; set; } = null!;
   public DataTemplate DecimalTemplate { get; set; } = null!;
   public DataTemplate FloatTemplate { get; set; } = null!;
   public DataTemplate CollectionTemplate { get; set; } = null!;
   public DataTemplate ObjectTemplate { get; set; } = null!;
   public DataTemplate DefaultTemplate { get; set; } = null!;

   public override DataTemplate SelectTemplate(object? item, DependencyObject container)
   {
      if (item is not PropertyItem property)
         return DefaultTemplate;

      var type = Nullable.GetUnderlyingType(property.Type) ?? property.Type;

      if (type == typeof(float))
         return FloatTemplate;

      if (type == typeof(string))
         return StringTemplate;

      if (type == typeof(bool))
         return BoolTemplate;

      if (type.IsEnum)
         return EnumTemplate;

      if (type == typeof(int) || type == typeof(long) || type == typeof(short))
         return IntTemplate;

      if (type == typeof(double) || type == typeof(decimal))
         return DecimalTemplate;

      if (type.IsArray ||
          type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) ||
          type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>) ||
          type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
         return CollectionTemplate;
      
      if (type.IsClass || type.IsValueType)
         return ObjectTemplate;

      return DefaultTemplate;
   }
}