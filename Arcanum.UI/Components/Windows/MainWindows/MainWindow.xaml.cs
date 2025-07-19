using System.Windows;
using System.Windows.Input;
using Arcanum.UI.Components.MVVM.Views.MainWindow;

namespace Arcanum.UI.Components.Windows.MainWindows;

public partial class MainWindow
{
   private readonly MainWindowView _view;

   public MainWindow()
   {
      InitializeComponent();

      _view = DataContext as MainWindowView ??
              throw new InvalidOperationException("DataContext is not set or is not of type MainWindowView.");
   }

   public void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
   {
      Console.WriteLine("Close command executed.");
      Application.Current.Shutdown();
   }

   private void CanCloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
}