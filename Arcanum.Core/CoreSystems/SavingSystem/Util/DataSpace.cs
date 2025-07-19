namespace Arcanum.Core.CoreSystems.SavingSystem.Util;

/// <summary>
/// Defines the root of a file structure.
/// Might either be a read-only directory or a read-write directory.
/// Base-Mods and Vanilla are implemented as read-only directories.
/// The current Mod is implemented as a read-write directory.
/// </summary>
public class DataSpace(string name, string[] path, DataSpace.AccessType access)
{
    public enum AccessType
    {
        ReadOnly,
        ReadWrite,
    }
    
    public static readonly DataSpace Empty = new DataSpace(string.Empty, ["ThisShouldNotExist"], AccessType.ReadOnly);
    
    public readonly AccessType Access = access;
    public readonly string[] Path = path;
    public readonly string Name = name;
}