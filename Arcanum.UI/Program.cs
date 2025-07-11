using Arcanum.Core.CoreSystems.ProjectFileUtil;

namespace Arcanum.UI
{
   internal static class Program
   {
      /// <summary>
      ///  The main entry point for the application.
      /// </summary>
      [STAThread]
      private static void Main()
      {
         ProjectFileUtil.GatherFilesForProjectFile(null);

         //Application.Run(new Form1());

         // var app = new Application();
         //
         // var window = new ExampleWindow();
         // app.Run(window);
         //
         // return;
         // var pluginHost = new PluginHost.PluginHost();
         // var lifecycleManager = new LifecycleManager();
         // lifecycleManager.RunStartUpSequence(pluginHost);

         // var consoleUi = new ConsoleUi.ConsoleUi("ExampleConsole", pluginHost);
         // DefaultCommands.RegisterDefaultCommands(consoleUi.ConsoleService, DefaultCommands.CommandCategory.All);
         // consoleUi.ShowDialog();

         //lifecycleManager.RunShutdownSequence(pluginHost);
      }
   }
}