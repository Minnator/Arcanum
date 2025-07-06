// In PluginA:

using Arcanum.API;
using Arcanum.API.CrossPluginServices;

namespace _2ndPlugin;

public interface IMyCalculationService
{
   int Add(int a, int b);
}

public class MyCalculationServiceImpl : IMyCalculationService
{
   public int Add(int a, int b) => a + b;
}

public class PluginLibService : IPluginLibrary
{
   // ... other IPlugin members
   private IPluginHost _host = null!;
   private IMyCalculationService _myServiceInstance = null!;

   public PluginStatus Status { get; set; }
   public bool IsActive { get; set; }
   public PluginRuntimeInfo RuntimeInfo { get; set; } = null!;
   public string AssemblyPath { get; set; } = null!;

   public bool Initialize(IPluginHost host)
   {
      _host = host;
      _myServiceInstance = new MyCalculationServiceImpl();
      var ipc = _host.GetService<ICrossPluginCommunicator>();
      ipc.PublishService(this, _myServiceInstance);
      return true;
   }

   public void OnEnable()
   {
   }

   public void OnDisable()
   {
   }

   public void Dispose()
   {
   }

   // ...
   public Guid Guid { get; } = Guid.Parse("d3b320c2-4c1e-4f5a-9bee-7f8c9d0e1a2b");
   public Version PluginVersion { get; } = new(1, 3, 0, 0);
   public Version RequiredHostVersion { get; } = new(1, 0, 0, 0);
   public string Name { get; } = "Plugin Service Publisher";
   public string Author { get; } = "Melonnator";
   public IEnumerable<IPluginMetadata.PluginDependency> Dependencies { get; } = [];
   // We let the plugin host manage the unloading of services
   public bool AutoUnloadServices { get; set; } = true;

   public void Log(string message, LoggingVerbosity verbosity = LoggingVerbosity.Info)
   {
      _host.Log("IPlugLib", message, verbosity);
   }
}