// using System.Windows;
// using Arcanum.Core.FlowControlServices;
//
// using ExampleWindow = Arcanum.UI.WpfTesting.ExampleWindow;
//
// namespace Arcanum.UI
// {
//    internal static class OldProgram
//    {
//       /// <summary>
//       ///  The main entry point for the application.
//       /// </summary>
//       [STAThread]
//       static void Main()
//       {
//          var app = new Application();
//
//          var window = new ExampleWindow();
//          app.Run(window);
//
//          return;
//
//          //Application.Run(new Form1());
//
//          var pluginHost = new PluginHost.PluginHost();
//          var lifecycleManager = new LifecycleManager();
//          lifecycleManager.RunStartUpSequence(pluginHost);
//
//          // var consoleUi = new ConsoleUi.ConsoleUi("ExampleConsole", pluginHost);
//          // DefaultCommands.RegisterDefaultCommands(consoleUi.ConsoleService, DefaultCommands.CommandCategory.All);
//          // consoleUi.ShowDialog();
//
//          lifecycleManager.RunShutdownSequence(pluginHost);
//       }
//    }
// }