using System.Diagnostics;
using Arcanum.Core.CoreSystems.IO;
using Arcanum.Core.CoreSystems.ParsingSystem;
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
         var inputPath = "C:\\Users\\david\\Downloads\\ParsingExample.txt";
         var (blocks, contents) = ElementParser.GetElements(inputPath,
                                                            IO.ReadAllTextUtf8(inputPath));

         Console.WriteLine($"Parsed Blocks: {blocks.Count}\nParsed Contents: {contents.Count}");

         //ProjectFileUtil.GatherFilesForProjectFile(null);

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