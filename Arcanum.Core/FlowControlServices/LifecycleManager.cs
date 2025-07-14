﻿using Arcanum.API;
using Arcanum.API.Console;
using Arcanum.API.Core.IO;
using Arcanum.Core.CoreSystems.ConsoleServices;
using Arcanum.Core.CoreSystems.IO;
using Arcanum.Core.CoreSystems.ProjectFileUtil;
using Arcanum.Core.PluginServices;

namespace Arcanum.Core.FlowControlServices;

public class LifecycleManager
{
   /* --- Bootup Sequence ---
    *
    *  0. Initialization of the plugin host
    *  1. Initialization of the core services
    *  2. Plugin host service initialization
    *  3. Loading of configuration settings
    *  4. Plugin discovery and loading, Initialization and enabling of plugins
    *  5. Showing of MainMenu
    *  Here it has several options:
    *    - Load game data -> Default editor
    *    - Map editor
    *    - Plugin manager
    *    - Project files manager
    */

   private PluginManager _pluginManager = null!;

   public void RunStartUpSequence(IPluginHost host)
   {
      InitializeApplicationCore();
      // Step 1: Initialize core services
      InitializeCoreServices(host);

      // Step 2: Initialize plugin host services
      host.RegisterDefaultServices();

      // Step 3: Load configuration settings
      //host.LoadConfigurationSettings();

      // Step 4: Discover, load and enable plugins
      _pluginManager = new(host);
      _pluginManager.LoadAndInitializePlugins();

      // Step 5: Show the main menu or UI
      //host.ShowMainMenu();
   }

#if DEBUG
   public void InsertPluginForTesting(IPlugin plugin)
   {
      if (_pluginManager == null)
         throw new InvalidOperationException("PluginManager is not initialized. Call RunStartUpSequence first.");

      _pluginManager.InjectPluginForTesting(plugin);
   }
#endif

   public void RunShutdownSequence(IPluginHost host)
   {
      // Step 1: Unload plugins
      _pluginManager.UnloadAll();

      // Step 2: Unload core services
      host.Unload();

      // Step 3: Perform any additional cleanup if necessary
      // This might include saving state, closing files, etc.

      // Shutdown the core application
      ArcanumDataHandler.SaveAllGitData(new());
   }

   private static void InitializeApplicationCore()
   {
      ArcanumDataHandler.LoadDefaultDescriptor(new());
   }

   private static void InitializeCoreServices(IPluginHost host)
   {
      // Initialize core services here
      // This might include logging, configuration management, etc.
      host.RegisterService<IFileOperations>(new APIWrapperIO());
      host.RegisterService<IJsonProcessor>(new APIWrapperJsonProcessor());
      host.RegisterService<IConsoleService>(new ConsoleServiceImpl(host, "ArcanumConsole"));
   }
}