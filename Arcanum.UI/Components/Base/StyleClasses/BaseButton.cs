using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Arcanum.UI.Components.Base.StyleClasses;

public class BaseButton : Button
{
    public static readonly DependencyProperty HoverBackgroundProperty =
        DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(BaseButton), new (Brushes.Transparent));
    public Brush HoverBackground
    {
        get => (Brush)GetValue(HoverBackgroundProperty);
        set => SetValue(HoverBackgroundProperty, value);
    }
}