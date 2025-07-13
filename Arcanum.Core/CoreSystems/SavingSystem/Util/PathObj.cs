using System.IO;

namespace Arcanum.Core.CoreSystems.SavingSystem.Util;

/// <summary>
/// Represents a Path separated into a root position, local path, and filename.
/// </summary>
/// <param name="localPath">
///     The local path components as an array of strings.
/// </param>
/// <param name="filename">
///     The filename as a string including the file extension.
/// </param>
/// <param name="rootPosition">
///     The root position indicating whether the path is relative to the vanilla game files or a mod.
/// </param>
public class PathObj(string[] localPath, string filename, FileManager.RootPosition rootPosition)
{
    public static readonly PathObj Empty = new PathObj([], string.Empty, FileManager.RootPosition.Vanilla);

    public readonly string[] LocalPath = localPath;
    public readonly string Filename = filename;
    public readonly FileManager.RootPosition RootPosition = rootPosition;
    
    /// <summary>
    /// The full path as a string, combining the root position, local path, and filename.
    /// </summary>
    public string FullPath => string.Join(Path.DirectorySeparatorChar, FileManager.GetPath(RootPosition), LocalPath, Filename);
}