using System.Windows.Input;

namespace Arcanum.Core.Globals.BackingClasses.WindowKeyBinds;

public class MainWindowKeyBinds
{
   public KeyGesture CloseProject { get; set; } = new (Key.O, ModifierKeys.Control);
}