using Application = System.Windows.Application;
using Timer = System.Timers.Timer;

namespace Arcanum.Core.Utils;

public class DelayedEvent<T> : IDisposable where T : EventArgs
{
   private bool _disposed;
   
   private readonly Timer _timer;
   private readonly object _lock = new();

   private T? _lastEventArgs;
   private object? _lastSender;

   private Action<object?, T>? _eventHandler;

   public event Action<object?, T>? EventHandler
   {
      add => _eventHandler += value;
      remove => _eventHandler -= value;
   }

   public DelayedEvent(int millisecondsDelay)
   {
      _timer = new(millisecondsDelay);
      _timer.AutoReset = false;
      _timer.Elapsed += (_, _) => OnElapsed();
   }

   public void AddHandler(Action<object?, T> handler)
   {
      EventHandler += handler;
   }

   public void RemoveHandler(Action<object?, T> handler)
   {
      EventHandler -= handler;
   }

   public void Invoke(object? sender, T args)
   {
      lock (_lock)
      {
         if (_disposed)
            return;

         _lastEventArgs = args;
         _lastSender = sender;

         _timer.Stop();
         _timer.Start();
      }
   }

   public void Cancel()
   {
      lock (_lock)
      {
         _timer.Stop();
         _lastEventArgs = null;
         _lastSender = null;
      }
   }

   private void OnElapsed()
   {
      object? sender;
      T? args;

      lock (_lock)
      {
         sender = _lastSender;
         args = _lastEventArgs;
         _lastEventArgs = null;
         _lastSender = null;
      }

      if (args != null)
      {
         var dispatcher = Application.Current?.Dispatcher;

         if (dispatcher != null)
            dispatcher.Invoke(() => _eventHandler?.Invoke(sender, args));
         else
            // fallback if no dispatcher (e.g. in unit test)
            _eventHandler?.Invoke(sender, args);
      }
   }

   public void Dispose()
   {
      lock (_lock)
      {
         if (_disposed)
            return;

         _timer.Dispose();
         _disposed = true;
      }
      GC.SuppressFinalize(this);
   }
}