namespace Arcanum.UI.Components.Base.StyleClasses;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// A TextBox with rounded corners.
/// </summary>
public class CorneredTextBox : TextBox
{
    public CornerRadius CornerRadiusValue
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }
    
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadiusValue), typeof(CornerRadius), typeof(CorneredTextBox), new (new CornerRadius(3)));
    
    public static readonly DependencyProperty HighlightOnFocusProperty =
        DependencyProperty.Register(nameof(HighlightOnFocus), typeof(bool), typeof(CorneredTextBox), new (true));

    public bool HighlightOnFocus
    {
        get => (bool)GetValue(HighlightOnFocusProperty);
        set => SetValue(HighlightOnFocusProperty, value);
    }

}