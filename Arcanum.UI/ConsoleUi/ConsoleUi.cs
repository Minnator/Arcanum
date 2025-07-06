// using Arcanum.API.Console;
// using Arcanum.Core.CoreSystems.ConsoleServices;
//
// namespace Arcanum.UI.ConsoleUi;
//
// public partial class ConsoleUi : Form
// {
//    private readonly InputTextBox _inputTextBox;
//    public IConsoleService ConsoleService { get; }
//
//    public ConsoleUi(string identifier, PluginHost.PluginHost host)
//    {
//       InitializeComponent();
//
//       OutputBox.BackColor = AcnColors.AcnColors.ConsoleBackground;
//       OutputBox.ForeColor = AcnColors.AcnColors.ConsoleText;
//       OutputBox.Font = new("Consolas", 9);
//
//       ConsoleService = new ConsoleServiceImpl(host, identifier, OutputBox);
//       _inputTextBox = new(ConsoleService)
//       {
//          Dock = DockStyle.Bottom,
//          BackColor = AcnColors.AcnColors.ConsoleBackground,
//          ForeColor = AcnColors.AcnColors.ConsoleText,
//          Font = new("Consolas", 9),
//          BorderStyle = BorderStyle.None,
//          SelectionStart = 0,
//          SelectionLength = 0,
//          Text = ConsoleServiceImpl.CMD_PREFIX,
//          Margin = new (3, 0, 3, 3)
//       };
//
//       MainTlp.Controls.Add(_inputTextBox, 0, 1);
//
//       MainTlp.BackColor = Color.Black;
//       
//       Load += ConsoleUi_Load;
//       FormClosing += ConsoleUi_FormClosing;
//    }
//    
//    private void ConsoleUi_Load(object? sender, EventArgs e)
//    {
//       _inputTextBox.Focus();
//       _inputTextBox.SelectionStart = _inputTextBox.Text.Length;
//       _inputTextBox.SelectionLength = 0;
//    }
//    
//    private void ConsoleUi_FormClosing(object? sender, FormClosingEventArgs e)
//    {
//       ConsoleService.Unload();
//    }
// }