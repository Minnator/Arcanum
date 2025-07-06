# Feature List of Project Arcanum
by Minnator and Melon Coaster

## Main Features
### Modular Design
The project is split up into four modules:
- `Arcanum.Core`: The core module that contains the main logic and data structures.
- `Arcanum.UI`: The user interface module that contains the UI components and logic.
- `Arcanum.API`: The API module that contains the public API for plugins and external access.
- `Arcanum.PluginHost`: The plugin host module that manages the loading, interaction, and unloading of plugins.

### Plugin System
The plugin system allows for the dynamic loading and unloading of plugins at runtime. Plugins can extend the functionality of the application by providing additional features, tools, or UI components.
- **Plugin Lifecycle Management**
The plugin system supports a clean lifecycle for plugins, including initialization and optional shutdown/cleanup hooks. This ensures that plugins can be properly managed and resources can be freed when they are no longer needed.
- **Unloading and Reloading**
  The plugin system supports unloading and reloading plugins at runtime. This allows for freeing resources or updating functionality without requiring a restart of the application. This feature is particularly useful for development and testing purposes.
- **Versioning and Compatibility**
  The plugin system implements version checks to ensure that plugins are compatible with the host application. This helps maintain stability and prevents issues caused by incompatible plugins.
- **Exception Isolation**
  The plugin system ensures that plugin errors are caught and logged without affecting the stability of the host application. This allows the host to remain responsive even if a plugin encounters an error.
- **Plugin Metadata System**
  The plugin system defines and exposes metadata such as name, version, and author to help identify and manage plugins. This metadata is essential for both the host application and users to understand the capabilities and compatibility of each plugin.
- **Event System**
The event system allows for communication between the host application and plugins. It supports strongly typed events to ensure type safety and flexibility.
- **Configuration Management**
The configuration management system allows plugins to store and retrieve configuration data. This is done through a dedicated section in the host's settings, ensuring that each plugin can maintain its own settings without interfering with others.
- **Dependency Management**
The plugin system supports loading plugins from isolated directories, allowing each plugin to have its own dependencies. This is achieved using separate assembly load contexts, ensuring that plugins do not conflict with each other or the host application.
- **UI Integration**
The plugin system provides hooks in the user interface for plugins to add buttons, panels, overlays, or other controls. This allows plugins to seamlessly integrate into the host application's UI. 
This also has the direct implication that the UI is designed to be generated on runtime and not pre-defined, allowing for dynamic changes based on the loaded plugins.


