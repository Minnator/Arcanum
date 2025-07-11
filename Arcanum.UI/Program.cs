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
         var hugeFile = "C:\\Users\\david\\Downloads\\100kLines.txt";

         var iterations = 1000;
         var (blocks, contents) = ElementParser.GetElements(inputPath, IO.ReadAllTextUtf8(hugeFile));
         Console.WriteLine($"Blocks: {blocks.Count}, Contents: {contents.Count}");
         var sw = new Stopwatch();
         sw.Start();
         for (var i = 0; i < iterations; i++)
            GetElementsRun(hugeFile);
         sw.Stop();
         Console.WriteLine($"in {sw.ElapsedMilliseconds/iterations}ms");

         
         
         /* Old Runtime for 100kLines with 1000 iterations
          * on average 55ms per iteration
          *  Changed string.Split to ReadonlySpan<char>.Slice and ReadOnlySpan<char>.IndexOf
          *  an average of 50ms per iteration
          *  Added method to trim StringBuilder without allocations
          *  an average of 45ms per iteration
          */
         
         
         
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

      private static void GetElementsRun(string filePath)
      {
         ElementParser.GetElements(filePath,
                                   IO.ReadAllTextUtf8(filePath));
      }
   }
}