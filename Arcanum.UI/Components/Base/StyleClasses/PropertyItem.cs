using System.Collections;
using System.ComponentModel;

namespace Arcanum.UI.Components.Base.StyleClasses;

public class PropertyItem : INotifyPropertyChanged
{
   public string Name { get; init; }
   public string Category { get; init; }
   public Type Type { get; init; }

   private object _value;
   public object Value
   {
      get => _value;
      set
      {
         if (_value == value)
            return;

         _value = value;
         PropertyChanged?.Invoke(this, new(nameof(Value)));
      }
   }

   public string CollectionDescription
   {
      get
      {
         if (_value is not ICollection collection)
            return string.Empty;

         var type = collection.GetType();
         var itemType = type.IsGenericType ? type.GetGenericArguments().FirstOrDefault() : typeof(object);

         return $"{type.Name}: ({collection.Count}) Items of {itemType?.Name}";
      }
   }

   public bool IsReadOnly { get; init; }

   public event PropertyChangedEventHandler? PropertyChanged;
}