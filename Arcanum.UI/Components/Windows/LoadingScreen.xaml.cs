using System.Windows.Threading;

namespace Arcanum.UI.Components.Windows;

public partial class LoadingScreen
{
   private readonly string[] _loadingTips =
   [
      "Press `F10` to view the error log", "Press `F1` to view the console",
      "Press `Shift + Shift` to open the application wide search: \"Queastor\"",
   ];
   
   private readonly DispatcherTimer _tipTimer = new();
   
   public string LoadingText 
   {
      get => LoadingTitleTextBlock.Text;
      set => LoadingTitleTextBlock.Text = value;
   }

   public LoadingScreen()
   {
      InitializeComponent();
      _tipTimer.Interval = TimeSpan.FromSeconds(5);
      _tipTimer.Tick += OnTipTimerOnTick;
      OnTipTimerOnTick(null, EventArgs.Empty); 
      _tipTimer.Start();
   }

   private void OnTipTimerOnTick(object? o, EventArgs eventArgs)
   {
      LoadingTipTextBlock.Text = _loadingTips[new Random().Next(_loadingTips.Length)];
   }
}