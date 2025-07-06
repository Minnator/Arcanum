using Arcanum.API;
using Arcanum.API.CrossPluginServices;

namespace _2ndPlugin;

public class PluginServiceConsumer : IPlugin
{
   public Guid Guid { get; } = Guid.Parse("d3b8f0c2-4c1e-4f5a-3bee-7f8c9d0e1a2b");
   public Version PluginVersion { get; } = new(1, 0, 56, 4);
   public Version RequiredHostVersion { get; } = new(1, 0, 0, 0);
   public string Name { get; } = "Plugin Service Consumer";
   public string Author { get; } = "Minnoaster";
   public IEnumerable<IPluginMetadata.PluginDependency> Dependencies { get; } = [];
   public PluginStatus Status { get; set; }
   public bool IsActive { get; set; }
   public required PluginRuntimeInfo RuntimeInfo { get; set; }
   public required string AssemblyPath { get; set; }
   private IPluginHost _host = null!;
   public bool Initialize(IPluginHost host)
   {
      _host = host;
      
      return true;
   }

   public void OnEnable()
   {
      var ipc = _host.GetService<ICrossPluginCommunicator>();
      var allCalcServices = ipc.GetPublishedServices<IMyCalculationService>();
      foreach (var (publisher, service) in allCalcServices)
         Log($"{Name}: Found Service from '{publisher.Name}'. Result: '{service.Add(10, 5)}'");
   }

   public void OnDisable()
   {
   }

   public void Dispose()
   {
   }

   public void Log(string message, LoggingVerbosity verbosity = LoggingVerbosity.Info) 
      => _host.Log(Name, message, verbosity);
}