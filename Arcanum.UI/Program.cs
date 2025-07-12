using System.Diagnostics;
using System.Windows;
using Arcanum.API.Settings;
using Arcanum.Core.CoreSystems.ConsoleServices;
using Arcanum.Core.CoreSystems.IO;
using Arcanum.Core.CoreSystems.ParsingSystem;
using Arcanum.Core.CoreSystems.ProjectFileUtil;
using Arcanum.Core.FlowControlServices;
using Arcanum.Core.PluginServices;
using Arcanum.UI.Components.Windows;
using Arcanum.UI.HostUIServices.SettingsGUI;
using Arcanum.UI.WpfTesting;

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
         var app = new Application();

         var resources = new[]
         {
            "Assets/ArcanumShared/DefaultPalette.xaml", "Assets/ArcanumShared/DefaultFonts.xaml",
            "Components/Base/Styles/BaseDarkButton.xaml", "Components/Base/Styles/BaseTextBoxStyle.xaml",
            "Components/Base/Styles/BaseComboboxStyle.xaml", "Components/Base/Styles/BorderlessComboBox.xaml",
            "Components/Base/Styles/BaseCheckBox.xaml", "Components/Base/Styles/BaseDarkScrollbar.xaml",
            "Components/Base/Styles/BaseListBox.xaml", "Components/Base/Styles/BaseTextBlock.xaml",
            "Components/Base/Styles/DarkTabControl.xaml", "Components/Base/Styles/StackPanelStyle.xaml",
         };

         foreach (var path in resources)
         {
            var dict = new ResourceDictionary { Source = new Uri(path, UriKind.Relative) };
            app.Resources.MergedDictionaries.Add(dict);
         }

         var pluginHost = new PluginHost.PluginHost();
         var lifecycleManager = new LifecycleManager();
         lifecycleManager.RunStartUpSequence(pluginHost);
         var examplePlugin = new ExamplePlugin();
         var motherOfAllPlugins = new TheMotherOfAllPluginNamesIsHere();
         
         var exampleDict = new Dictionary<Guid, IPluginSetting>
         {
            { examplePlugin.Guid, new ExamplePluginSettings() },
            { motherOfAllPlugins.Guid, new ExamplePluginSettings2() },
            
         };
         
         lifecycleManager.InsertPluginForTesting(examplePlugin);
         lifecycleManager.InsertPluginForTesting(motherOfAllPlugins);
         SettingsWindow.ShowSettingsWindow(exampleDict, Guid.Empty, pluginHost);
         
         // var consoleImpl = new ConsoleServiceImpl(pluginHost, "ExampleConsole");
         // var window = new ConsoleWindow(consoleImpl);
         //app.Run(new SettingsWindow());
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