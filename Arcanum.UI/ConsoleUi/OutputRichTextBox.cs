// using Arcanum.API.Console;
// using Arcanum.Core.CoreSystems.ConsoleServices;
//
// namespace Arcanum.UI.ConsoleUi;
//
// public sealed class OutputRichTextBox : UserControl, IOutputReceiver
// {
//    private readonly RichTextBox _richTextBox;
//
//    public OutputRichTextBox()
//    {
//       _richTextBox = new()
//       {
//          ReadOnly = true,
//          BorderStyle = BorderStyle.None,
//          BackColor = AcnColors.AcnColors.ConsoleBackground,
//          ForeColor = AcnColors.AcnColors.LightBlueText,
//          Font = new("Consolas", 10),
//          SelectionStart = 0,
//          SelectionLength = 0,
//          ScrollBars = RichTextBoxScrollBars.Vertical,
//          Dock = DockStyle.Fill,
//       };
//
//       Controls.Add(_richTextBox);
//    }
//
//    public void WriteLine(string message, bool scrollToCaret, bool prefix)
//    {
//       if (string.IsNullOrWhiteSpace(message))
//          return;
//
//       if (_richTextBox.Text.Length > 0 && !_richTextBox.Text.EndsWith(Environment.NewLine))
//          _richTextBox.Text += Environment.NewLine;
//
//       _richTextBox.Text += (prefix ? ConsoleServiceImpl.CMD_PREFIX : string.Empty) + message;
//       _richTextBox.SelectionStart = _richTextBox.Text.Length;
//       _richTextBox.SelectionLength = 0;
//       
//       if (scrollToCaret)
//          _richTextBox.ScrollToCaret();
//    }
//
//    public void WriteLines(List<string> messages)
//    {
//       if (messages.Count == 0)
//          return;
//
//       for (var i = 0; i < messages.Count; i++)
//       {
//          WriteLine(messages[i], false, i == 0);
//       }
//
//       _richTextBox.ScrollToCaret();
//    }
//
//    public void WriteError(string message)
//    {
//       if (string.IsNullOrWhiteSpace(message))
//          return;
//
//       if (_richTextBox.Text.Length > 0 && !_richTextBox.Text.EndsWith(Environment.NewLine))
//          _richTextBox.Text += Environment.NewLine;
//
//       _richTextBox.Text += ConsoleServiceImpl.CMD_PREFIX + "Error: " + message + Environment.NewLine;
//       _richTextBox.SelectionStart = _richTextBox.Text.Length;
//       _richTextBox.SelectionLength = 0;
//       _richTextBox.ScrollToCaret();
//    }
//
//    public void Clear()
//    {
//       _richTextBox.Text = string.Empty;
//       _richTextBox.SelectionStart = 0;
//       _richTextBox.SelectionLength = 0;
//    }
// }