namespace Arcanum.API.UtilServices;

/// <summary>
/// An interface all Services registered with the plugin host must implement to allow for proper unloading and cleanup.
/// </summary>
public interface IService
{
   /// <summary>
   /// Performs any necessary cleanup or resource deallocation
   /// when the service is being unloaded by the plugin host.
   /// </summary>
   public void Unload();
}