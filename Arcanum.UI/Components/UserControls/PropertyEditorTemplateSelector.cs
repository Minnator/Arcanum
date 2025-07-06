using System.Windows;
using System.Windows.Controls;
using Arcanum.UI.Components.Base.StyleClasses;

namespace Arcanum.UI.Components.UserControls;

public class PropertyEditorTemplateSelector : DataTemplateSelector
{
   public DataTemplate StringTemplate { get; set; } = null!;
   public DataTemplate BoolTemplate { get; set; } = null!;
   public DataTemplate EnumTemplate { get; set; } = null!;
   public DataTemplate IntTemplate { get; set; } = null!;
   public DataTemplate DefaultTemplate { get; set; } = null!;

   public override DataTemplate SelectTemplate(object? item, DependencyObject container)
   {
      if (item is not PropertyItem property)
         return DefaultTemplate;

      if (property.Type == typeof(string))
         return StringTemplate;

      if (property.Type == typeof(bool))
         return BoolTemplate;

      if (property.Type.IsEnum)
         return EnumTemplate;

      if (property.Type == typeof(int) || property.Type == typeof(long) || property.Type == typeof(short))
         return IntTemplate;

      return DefaultTemplate;
   }
}