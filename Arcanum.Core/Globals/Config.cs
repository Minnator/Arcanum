using Arcanum.Core.Globals.BackingClasses;

namespace Arcanum.Core.Globals;

/// <summary>
/// This contains all user-defined configuration settings.
/// </summary>
public static class Config
{
   public static UserKeyBinds UserKeyBinds { get; set; } = new ();
}