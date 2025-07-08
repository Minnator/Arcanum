using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Arcanum.UI.Components.Base.StyleClasses;

public class PropertyItem : INotifyPropertyChanged
{
   public string Name { get; init; }
   public string Category { get; init; }
   public Type Type { get; init; }

   public bool IsReadOnly { get; init; }

   private readonly Func<object> _getter;
   private readonly Action<object>? _setter;

   public PropertyItem(string name, Type type, Func<object> getter, Action<object>? setter = null, string category = "")
   {
      Name = name;
      Type = type;
      _getter = getter;
      _setter = setter;
      Category = category;
      IsReadOnly = setter == null;
   }

   public object Value
   {
      get => _getter();
      set
      {
         if (IsReadOnly || Equals(_getter(), value))
            return;

         _setter?.Invoke(value);
         PropertyChanged?.Invoke(this, new(nameof(Value)));
      }
   }

   public string CollectionDescription
   {
      get
      {
         if (_getter() is not ICollection collection)
            return string.Empty;

         var type = collection.GetType();
         var itemType = type.IsGenericType ? type.GetGenericArguments().FirstOrDefault() : typeof(object);
         return $"{type.Name}: ({collection.Count}) Items of {itemType?.Name}";
      }
   }

   public event PropertyChangedEventHandler? PropertyChanged;
   
   public static PropertyItem FromExpression<TModel, TProp>(TModel instance, Expression<Func<TModel, TProp>> expr)
   {
      var member = (MemberExpression)expr.Body;
      var propInfo = (PropertyInfo)member.Member;

      return new(
                 name: propInfo.Name,
                 type: typeof(TProp),
                 getter: () => propInfo.GetValue(instance)!,
                 setter: v => propInfo.SetValue(instance, Convert.ChangeType(v, propInfo.PropertyType))
                );
   }

}
