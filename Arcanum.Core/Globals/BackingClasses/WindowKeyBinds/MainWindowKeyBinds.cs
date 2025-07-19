using System.Windows.Input;
using Arcanum.API.Core.KeyBinds;

namespace Arcanum.Core.Globals.BackingClasses.WindowKeyBinds;

public class MainWindowKeyBinds : KeyBindProvider
{
   public KeyGesture CloseProject { get; set; } = new (Key.O, ModifierKeys.Control);
}