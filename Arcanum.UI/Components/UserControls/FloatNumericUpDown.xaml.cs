using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arcanum.UI.Components.UserControls;

public partial class FloatNumericUpDown : UserControl
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
                                  new FrameworkPropertyMetadata(0.0, OnMinMaxChanged));

   public float MaxValue
   {
      get => (float)GetValue(MaxValueProperty);
      set => SetValue(MaxValueProperty, value);
   }

   public static readonly DependencyProperty MaxValueProperty =
      DependencyProperty.Register(nameof(MaxValue),
                                  typeof(float),
                                  typeof(FloatNumericUpDown),
                                  new FrameworkPropertyMetadata(100.0, OnMinMaxChanged));

   public float Value
   {
      get => (float)GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }

   public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register(nameof(Value),
                                  typeof(float),
                                  typeof(FloatNumericUpDown),
                                  new FrameworkPropertyMetadata(10.0,
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
                                  new FrameworkPropertyMetadata(0.1));

   private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      var control = (FloatNumericUpDown)d;
      var newValue = (float)e.NewValue;

      if (newValue < control.MinValue)
         control.Value = control.MinValue;
      else if (newValue > control.MaxValue)
         control.Value = control.MaxValue;
      else
      {
         var formatted = newValue.ToString("F2", CultureInfo.InvariantCulture);
         if (control.NudTextBox.Text != formatted!)
            control.NudTextBox.Text = formatted;
      }
   }

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

   private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
   {
      if (!float.TryParse(NudTextBox.Text, CultureInfo.InvariantCulture, out var number))
         number = 0;

      number += StepSize;

      if (number > MaxValue)
         number = MaxValue;

      NudTextBox.Text = number.ToString("F2", CultureInfo.InvariantCulture);
   }

   private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
   {
      if (!float.TryParse(NudTextBox.Text, CultureInfo.InvariantCulture, out var number))
         number = 0;

      number -= StepSize;

      if (number < MinValue)
         number = MinValue;

      NudTextBox.Text = number.ToString("F2", CultureInfo.InvariantCulture);
   }

   private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
   {
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
}