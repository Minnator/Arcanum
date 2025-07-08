using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Arcanum.UI.Components.UserControls;

public partial class BaseNumericUpDown
{
   public BaseNumericUpDown()
   {
      InitializeComponent();
      NudTextBox.Text = Value.ToString();
   }

   public int MinValue
   {
      get => (int)GetValue(MinValueProperty);
      set => SetValue(MinValueProperty, value);
   }

   public static readonly DependencyProperty MinValueProperty =
      DependencyProperty.Register(nameof(MinValue),
                                  typeof(int),
                                  typeof(BaseNumericUpDown),
                                  new FrameworkPropertyMetadata(0, OnMinMaxChanged));

   public int MaxValue
   {
      get => (int)GetValue(MaxValueProperty);
      set => SetValue(MaxValueProperty, value);
   }

   public static readonly DependencyProperty MaxValueProperty =
      DependencyProperty.Register(nameof(MaxValue),
                                  typeof(int),
                                  typeof(BaseNumericUpDown),
                                  new FrameworkPropertyMetadata(100000, OnMinMaxChanged));

   public int Value
   {
      get => (int)GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }

   public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register(nameof(Value),
                                  typeof(int),
                                  typeof(BaseNumericUpDown),
                                  new FrameworkPropertyMetadata(10,
                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                OnValueChanged));

   private void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
   {
      if (e.Key == Key.Up)
      {
         NudButtonUp.RaiseEvent(new(ButtonBase.ClickEvent));
         typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)!
                       .Invoke(NudButtonUp, [true]);
      }

      if (e.Key == Key.Down)
      {
         NudButtonDown.RaiseEvent(new(ButtonBase.ClickEvent));
         typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)!
                       .Invoke(NudButtonDown, [true]);
      }
   }

   private void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
   {
      if (e.Key == Key.Up)
         typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)!
                       .Invoke(NudButtonUp, [false]);

      if (e.Key == Key.Down)
         typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)!
                       .Invoke(NudButtonDown, [false]);
   }

   private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      var control = (BaseNumericUpDown)d;

      if (control.MinValue > control.MaxValue)
         control.MinValue = control.MaxValue;

      if (control.Value < control.MinValue)
         control.Value = control.MinValue;
      else if (control.Value > control.MaxValue)
         control.Value = control.MaxValue;
   }

   private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      var control = (BaseNumericUpDown)d;
      var newValue = (int)e.NewValue;

      // Clamp value once here, but do NOT set Value again inside OnValueChanged
      if (newValue < control.MinValue)
         newValue = control.MinValue;
      else if (newValue > control.MaxValue)
         newValue = control.MaxValue;

      // Update TextBox text only if different
      if (control.NudTextBox.Text != newValue.ToString())
         control.NudTextBox.Text = newValue.ToString();
   }

   private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
   {
      if (int.TryParse(NudTextBox.Text, out var number) && number < MaxValue)
         SetCurrentValue(ValueProperty, number + 1);
   }

   private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
   {
      if (int.TryParse(NudTextBox.Text, out var number) && number > MinValue)
         SetCurrentValue(ValueProperty, number - 1);
   }

   private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
   {
      if (int.TryParse(NudTextBox.Text, out var number))
      {
         if (number < MinValue)
            number = MinValue;
         if (number > MaxValue)
            number = MaxValue;
         SetCurrentValue(ValueProperty, number);
      }
      else
      {
         // Revert text to last valid value
         NudTextBox.Text = Value.ToString();
         NudTextBox.SelectionStart = NudTextBox.Text.Length;
      }
   }

   private void NudTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
   {
      if (e.Delta > 0)
         NUDButtonUP_Click(sender, e);
      else
         NUDButtonDown_Click(sender, e);
   }
}