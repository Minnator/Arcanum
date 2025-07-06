using Arcanum.API;
using Arcanum.API.Events;
using Arcanum.API.Settings;
using Arcanum.API.UtilServices;
using Arcanum.PluginHost.PluginServices;
using Arcanum.PluginHost.Settings;

namespace Arcanum.PluginHost;

public static class HostInfo
{
   public static Version Version => new(1, 0, 0, 0);
}

public class PluginHost : IPluginHost
{
   private readonly Dictionary<Type, IService> _services = new();
   private readonly ISettingsUiService _uiSettingsService;

   public PluginHost()
   {
      // TODO use WPF instead of WinForms
      //_uiSettingsService = uiSettingsService ?? throw new ArgumentNullException(nameof(uiSettingsService), "UI Settings Service cannot be null.");
   }

   public void RegisterDefaultServices()
   {
      RegisterService<IEventBus>(EventBus.Instance);
      RegisterService<ISettingsStore>(new SettingsStore(_uiSettingsService, this));
   }

   public void Unload()
   {
      foreach (var service in _services.Values)
         service.Unload();
      _services.Clear();
   }

   public string GuidToName(Guid guid)
   {
      return GetService<IPluginInfoService>().GetPluginByGuid(guid)?.Name
         ?? throw new InvalidOperationException($"No plugin found with GUID: {guid}");
   }

   public T GetService<T>() where T : class, IService
   {
      if (_services.TryGetValue(typeof(T), out var service))
         return (T)service;

      throw new InvalidOperationException($"Service of type {typeof(T).Name} is not (yet?) registered.");
   }

   public void RegisterService<T>(T service) where T : class, IService
   {
      if (_services.ContainsKey(typeof(T)))
         throw new InvalidOperationException($"Service of type {typeof(T).Name} is already registered.");

      _services[typeof(T)] = service ?? throw new ArgumentNullException(nameof(service), "Service cannot be null.");
   }

   public void Log(string message, LoggingVerbosity verbosity = LoggingVerbosity.Info)
      => Console.WriteLine($"[{GetVerbosityPrefix(verbosity)}] [PluginHost] {message}");

   public void Log(string subRoutinePreFix, string message, LoggingVerbosity verbosity = LoggingVerbosity.Info)
      => Console.WriteLine($"[{GetVerbosityPrefix(verbosity)}] [PluginHost.{subRoutinePreFix[..8]}] {message}");

   private string GetVerbosityPrefix(LoggingVerbosity verbosity)
   {
      return verbosity switch
      {
         LoggingVerbosity.Info => "Inf",
         LoggingVerbosity.Warning => "War",
         LoggingVerbosity.Error => "Err",
         _ => "Unk",
      };
   }
}