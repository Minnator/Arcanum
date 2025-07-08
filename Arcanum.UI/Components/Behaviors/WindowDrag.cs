using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Arcanum.UI.Components.Behaviors;

public class WindowDrag : Behavior<FrameworkElement>
{
   private Point? _lastMousePosition;
   private Window _window = null!;

   protected override void OnAttached()
   {
      var nullableWindow = Window.GetWindow(AssociatedObject);
      if (nullableWindow is null)
         return;

      _window = nullableWindow;
      base.OnAttached();
      AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
      AssociatedObject.MouseMove += AssociatedObject_MouseMove;
   }

   protected override void OnDetaching()
   {
      base.OnDetaching();
      AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
   }

   private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
   {
      if (e.ClickCount == 2)
      {
         if (_window.WindowState == WindowState.Normal)
            _window.WindowState = WindowState.Maximized;
         else if (_window.WindowState == WindowState.Maximized)
            _window.WindowState = WindowState.Normal;
      }
      else if (_window.WindowState == WindowState.Maximized)
         _lastMousePosition ??= e.GetPosition(_window);
      // double click
   }

   private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
   {
      if (e.LeftButton != MouseButtonState.Pressed)
         return;

      // If we are in a maximized state, we only want to move the window once we drag it, not just by clicking 
      if (_window.WindowState == WindowState.Maximized)
      {
         var position = e.GetPosition(_window);
         if (_lastMousePosition != null && _lastMousePosition != position)
         {
            var mousePosition = _window.PointToScreen(position);

            // Calculate the desired left offset so that the mouse remains at the same
            // relative position on the window after restoring
            var targetLeft = mousePosition.X - _window.RestoreBounds.Width / 2;

            _window.WindowState = WindowState.Normal;

            // Set windows left, so center is under mouse
            _window.Left = targetLeft;
            _window.Top = mousePosition.Y - position.Y; // preserve Y coordinate

            _lastMousePosition = null;
         }
      }

      _window.DragMove();
   }
}