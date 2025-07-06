using System.ComponentModel;
using Arcanum.API;
using Arcanum.API.Events;
using Arcanum.API.Settings;

namespace PluginTest;

[PluginSettingKey("SettingsObjOne")]
internal class TestSetting(Guid guid) : IPluginSetting
{
   [Category("General")]
   [Description("A test setting for the plugin system.")]
   [DefaultValue("Default Value")]
   public string Key { get; set; } = string.Empty;
   public string PluginName { get; set; } = string.Empty;
   public object Value { get; set; } = null!;
   public string Description { get; set; } = string.Empty;
   public bool RaiseEvent { get; set; } = true;

   public TestSetting(string key,
                      string pluginName,
                      object value,
                      string description,
                      Guid guid,
                      bool raiseEvent = true) : this(guid)
   {
      Key = key;
      PluginName = pluginName;
      Value = value;
      Description = description;
      RaiseEvent = raiseEvent;
   }

   public Guid OwnerGuid { get; } = guid;
}

class MyComponent(Guid guid) : Component, IPluginSetting
{
   private int _value = 42;

   [DefaultValue(42)]
   public int Value
   {
      get => _value;
      set => _value = value;
   }

   public bool ShouldSerializeValue() => _value != 42;
   public void ResetValue() => _value = 42;
   public Guid OwnerGuid { get; } = guid;
}

public class TestPlugin : IPlugin
{
   private IPluginHost _host = null!;

   public Guid Guid { get; } = Guid.Parse("63b8f0c2-4c1e-4f5a-9b6e-7f8c9d0e1a2b");
   public Version PluginVersion { get; } = new(1, 0, 0, 0);
   public Version RequiredHostVersion { get; } = new(1, 0, 0, 0);
   public string Name => "Test Plugin";
   public string Author => "Minnator";
   public IEnumerable<IPluginMetadata.PluginDependency> Dependencies { get; } = [];
   public PluginRuntimeInfo RuntimeInfo { get; set; } = null!;
   public required string AssemblyPath { get; set; }
   public PluginStatus Status { get; set; }
   public bool IsActive { get; set; }

   public enum SettingsTest
   {
      Value1,
      Value2,
      Value3,
      Cheeseeeeeee,
   }

   public bool Initialize(IPluginHost host)
   {
      _host = host;
      var eventBus = _host.GetService<IEventBus>();
      eventBus.Register<EventArgs>(PluginEventId.Load_After_DataLoaded,
                                   e => { host.Log(nameof(TestPlugin), $"base Event Registered: {e.GetType()}"); });

      var settings = _host.GetService<ISettingsStore>();

      //var testSetting = new TestSetting("TestValue", Name, SettingsTest.Value1,
      //                                   "A test setting with an enum value", Guid);

      //settings.Set(Guid, testSetting);

      var myComponent = new MyComponent(Guid);
      settings.Set(Guid, myComponent);

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
      /* cleanup */
   }

   public void Log(string message, LoggingVerbosity verbosity = LoggingVerbosity.Info)
   {
      _host.Log(nameof(TestPlugin), message, verbosity);
   }
}

public class Test2Plugin : IPlugin
{
   private IPluginHost _host = null!;

   public Guid Guid { get; } = Guid.Parse("21b2c3d4-e5f6-7e8e-9e0e-e1e2e3e4e5e6");
   public Version PluginVersion { get; } = new(1, 0, 0, 0);
   public Version RequiredHostVersion { get; } = new(1, 0, 0, 0);
   public string Name => "Super ultra hack plugin";
   public string Author => "Free provinces to go";
   public IEnumerable<IPluginMetadata.PluginDependency> Dependencies { get; } = [];
   public PluginRuntimeInfo RuntimeInfo { get; set; } = null!;
   public required string AssemblyPath { get; set; }

   public PluginStatus Status { get; set; }
   public bool IsActive { get; set; }

   public bool Initialize(IPluginHost host)
   {
      _host = host;

      var settings = _host.GetService<ISettingsStore>();

      settings.SettingsUi.SettingChanged += (_, e) =>
      {
         _host.Log(nameof(TestPlugin), $"Setting changed: Plugin: {e.PluginName} => {e.SettingKey} = {e.Value}");
      };

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
      /* cleanup */
   }

   public void Log(string message, LoggingVerbosity verbosity = LoggingVerbosity.Info)
   {
      _host.Log(nameof(Test2Plugin), message, verbosity);
   }
}