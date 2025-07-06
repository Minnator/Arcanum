using System.ComponentModel;
using Arcanum.API;
using Arcanum.API.Events;
using Arcanum.API.Settings;

namespace _2ndPlugin;

[PluginSettingKey("SettingsFor2NdPluginObj")]
public class SettingsFor2NdPlugin : IPluginSetting
{
   public string Key { get; set; } 
   public string DisplayName { get; set; }
   public string Description { get; set; }
   [DefaultValue(false)]
   public bool DefaultValue { get; set; } 
   public Type ValueType { get; set; } 
   public bool IsVisible { get; set; } = true;

   public SettingsFor2NdPlugin(string key,
                               string displayName,
                               string description,
                               bool defaultValue,
                               Type valueType,
                               Guid ownerGuid)
   {
      Key = key;
      DisplayName = displayName;
      Description = description;
      DefaultValue = defaultValue;
      ValueType = valueType;
      OwnerGuid = ownerGuid;
   }

   public Guid OwnerGuid { get; }
}

public class _2ndPlugin : IPlugin
{
   public Guid Guid { get; } = Guid.Parse("d3b8f0c2-4c1e-4f5a-9bee-7f8c9d0e1a2b");
   public Version PluginVersion { get; } = new(1, 0, 0, 0);
   public Version RequiredHostVersion { get; } = new(1, 0, 0, 0);
   public string Name { get; } = "2nd Plugin";
   public string Author { get; } = "Melon Toaster";
   public IEnumerable<IPluginMetadata.PluginDependency> Dependencies { get; } = [];

   private IPluginHost _host = null!;
   public PluginStatus Status { get; set; }
   public bool IsActive { get; set; }
   public PluginRuntimeInfo RuntimeInfo { get; set; } = null!;
   public required string AssemblyPath { get; set; }

   public bool Initialize(IPluginHost host)
   {
      _host = host;
      var settings = _host.GetService<ISettingsStore>();

      settings.SettingChanged += OnSettingChanged;

      var setting = new SettingsFor2NdPlugin("ExamplePlugin.Enabled",
                                             "Enable Example Plugin",
                                             "Enables the Example Plugin functionality.",
                                             true,
                                             typeof(bool),
                                             Guid);

      settings.Set(Guid, setting);
      
      _host.Log(nameof(_2ndPlugin),
                $"Setting registered: ExamplePlugin.Enabled = {settings.Get<bool>(typeof(SettingsFor2NdPlugin).GetProperty(nameof(SettingsFor2NdPlugin.IsVisible))!, this)}");

      return true;
   }

   public void OnEnable()
   {
   }

   public void OnDisable()
   {
   }

   private void OnSettingChanged(object? sender, PluginSettingEventArgs e)
   {
      _host.Log($"Setting changed: {e.PropertyInfo?.Name} = {e.Value}");
   }

   public void Dispose()
   {
   }

   public void Log(string message, LoggingVerbosity verbosity = LoggingVerbosity.Info)
   {
      _host.Log(Name, message, verbosity);
   }
}