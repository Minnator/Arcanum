// using Arcanum.API.Console;
// using Arcanum.Core.CoreSystems.ConsoleServices;
//
// namespace Arcanum.UI.ConsoleUi;
//
// public class InputTextBox : TextBox
// {
//    private readonly IConsoleService _handler;
//
//    public InputTextBox(IConsoleService handler)
//    {
//       _handler = handler;
//
//       KeyDown += RichTextBox_KeyDown;
//       TextChanged += RichTextBox_TextChanged;
//    }
//
//    public new void Clear()
//    {
//       Text = ConsoleServiceImpl.CMD_PREFIX;
//       SelectionStart = Text.Length;
//       SelectionLength = 0;
//    }
//
//    private void RichTextBox_TextChanged(object? sender, EventArgs e)
//    {
//       if (Text.Length < ConsoleServiceImpl.CMD_PREFIX_LENGTH ||
//           Text.Length == ConsoleServiceImpl.CMD_PREFIX_LENGTH && !Text.Equals(ConsoleServiceImpl.CMD_PREFIX))
//       {
//          Text = ConsoleServiceImpl.CMD_PREFIX;
//          SelectionLength = 0;
//          SelectionStart = Text.Length;
//       }
//
//       if (!Text.StartsWith(ConsoleServiceImpl.CMD_PREFIX))
//       {
//          Text = ConsoleServiceImpl.CMD_PREFIX + Text;
//          SelectionLength = 0;
//          SelectionStart = Text.Length;
//       }
//    }
//
//    private void RichTextBox_KeyDown(object? sender, KeyEventArgs e)
//    {
//       if (e.KeyCode == Keys.Enter)
//       {
//          _handler.ProcessCommand(Text);
//          Clear();
//          e.SuppressKeyPress = true;
//          e.Handled = true;
//       }
//       else if (e.KeyCode == Keys.Back)
//       {
//          if (Text.Length == ConsoleServiceImpl.CMD_PREFIX_LENGTH) // we don't allow the user to delete the CMD_PREFIX chars
//          {
//             e.SuppressKeyPress = true;
//             e.Handled = true;
//          }
//       }
//       else if (e.KeyCode == Keys.Up)
//       {
//          if (_handler.HistoryIndex >= 0)
//             Text = ConsoleServiceImpl.CMD_PREFIX + _handler.GetPreviousHistoryEntry();
//          e.SuppressKeyPress = true;
//          e.Handled = true;
//          SelectionStart = Text.Length;
//       }
//       else if (e.KeyCode == Keys.Down)
//       {
//          if (_handler.HistoryIndex > _handler.GetHistory().Count - 1)
//             Text = ConsoleServiceImpl.CMD_PREFIX;
//          else
//             Text = ConsoleServiceImpl.CMD_PREFIX + _handler.GetNextHistoryEntry();
//          e.SuppressKeyPress = true;
//          e.Handled = true;
//          SelectionStart = Text.Length;
//       }
//    }
// }