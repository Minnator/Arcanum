using System.Windows.Input;

namespace Arcanum.UI.Components.MVVM.Views.MainWindow;

public static class MwCommands
{
   public static readonly RoutedCommand CloseProjectFileCommand = new("CloseProjectFileCommand", typeof(MainWindowView));
}