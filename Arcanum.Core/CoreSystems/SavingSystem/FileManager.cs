namespace Arcanum.Core.CoreSystems.SavingSystem;

/// <summary>
/// The FileManager class is responsible for managing file paths for both vanilla and modded content.
/// </summary>
public static class FileManager
{
    /// <summary>
    /// Enumeration representing the root position of a file path.
    /// </summary>
    public enum RootPosition
    {
        Vanilla,
        Mod,
    }

    /// <summary>
    /// Retrieves the file path based on the specified root position.
    /// </summary>
    /// <param name="rootPosition">
    /// Represents the root position of the file path, indicating whether it is relative to the vanilla game files or a mod.
    /// </param>
    /// <returns>
    /// String array representing the file path components based on the root position.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string[] GetPath(RootPosition rootPosition)
    {
        return rootPosition switch
        {
            RootPosition.Mod => ModPath,
            RootPosition.Vanilla => VanillaPath,
            _ => throw new ArgumentOutOfRangeException(nameof(rootPosition), rootPosition, null)
        };
    }
    
    public static string[] ModPath { get; set; }

    public static string[] VanillaPath { get;  set; }
}