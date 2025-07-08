using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arcanum.UI.Components.UserControls;

public partial class FloatNumericUpDown
{
   public FloatNumericUpDown()
   {
      InitializeComponent();
      DataObject.AddPastingHandler(NudTextBox, NudTextBox_Pasting);
   }

   public float MinValue
   {
      get => (float)GetValue(MinValueProperty);
      set => SetValue(MinValueProperty, value);
   }

   public static readonly DependencyProperty MinValueProperty =
      DependencyProperty.Register(nameof(MinValue),
                                  typeof(float),
                                  typeof(FloatNumericUpDown),
                                  new FrameworkPropertyMetadata(0.0f, OnMinMaxChanged));

   public float MaxValue
   {
      get => (float)GetValue(MaxValueProperty);
      set => SetValue(MaxValueProperty, value);
   }

   public static readonly DependencyProperty MaxValueProperty =
      DependencyProperty.Register(nameof(MaxValue),
                                  typeof(float),
                                  typeof(FloatNumericUpDown),
                                  new FrameworkPropertyMetadata(100.0f, OnMinMaxChanged));

   public float Value
   {
      get => (float)GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }

   public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register(nameof(Value),
                                  typeof(float),
                                  typeof(FloatNumericUpDown),
                                  new FrameworkPropertyMetadata(10.0f,
                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                OnValueChanged));

   public float StepSize
   {
      get => (float)GetValue(StepSizeProperty);
      set => SetValue(StepSizeProperty, value);
   }

   public static readonly DependencyProperty StepSizeProperty =
      DependencyProperty.Register(nameof(StepSize),
                                  typeof(float),
                                  typeof(FloatNumericUpDown),
                                  new FrameworkPropertyMetadata(0.1f));

   private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      var control = (FloatNumericUpDown)d;

      if (control.MinValue > control.MaxValue)
         control.MinValue = control.MaxValue;

      if (control.Value < control.MinValue)
         control.Value = control.MinValue;
      else if (control.Value > control.MaxValue)
         control.Value = control.MaxValue;
   }

   private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      var control = (FloatNumericUpDown)d;
      var newValue = (float)e.NewValue;

      // Clamp value once here, but do NOT set Value again inside OnValueChanged
      if (newValue < control.MinValue)
         newValue = control.MinValue;
      else if (newValue > control.MaxValue)
         newValue = control.MaxValue;

      // Update TextBox text only if different
      if (control.NudTextBox.Text != newValue.ToString(CultureInfo.InvariantCulture))
         control.NudTextBox.Text = newValue.ToString(CultureInfo.InvariantCulture);
   }

   private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
   {
      if (float.TryParse(NudTextBox.Text, CultureInfo.InvariantCulture, out var number) && number < MaxValue)
         SetCurrentValue(ValueProperty, number + StepSize);
   }

   private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
   {
      if (float.TryParse(NudTextBox.Text, CultureInfo.InvariantCulture, out var number) && number > MinValue)
         SetCurrentValue(ValueProperty, number - StepSize);
   }

   private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
   {
      if (float.TryParse(NudTextBox.Text, CultureInfo.InvariantCulture, out var number))
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
         NudTextBox.Text = Value.ToString(CultureInfo.InvariantCulture);
         NudTextBox.SelectionStart = NudTextBox.Text.Length;
      }
   }

   private void NudTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
   {
      var textBox = (TextBox)sender;
      var proposedText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

      if (!float.TryParse(proposedText,
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                          CultureInfo.InvariantCulture,
                          out _))
      {
         e.Handled = true;
      }
   }

   private void NudTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
   {
      if (e.DataObject.GetDataPresent(typeof(string)))
      {
         var pastedText = (string)e.DataObject.GetData(typeof(string))!;
         var textBox = (TextBox)sender;
         var proposedText = textBox.Text.Insert(textBox.SelectionStart, pastedText);

         if (!float.TryParse(proposedText,
                             NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                             CultureInfo.InvariantCulture,
                             out _))
         {
            e.CancelCommand();
         }
      }
      else
      {
         e.CancelCommand();
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