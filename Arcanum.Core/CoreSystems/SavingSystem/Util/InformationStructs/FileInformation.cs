namespace Arcanum.Core.CoreSystems.SavingSystem.Util.InformationStructs;

public readonly struct FileInformation(string fileName,
    FileTypeInformation fileType,
    PathObj savePath,
    bool allowsOverwrite)
{
    /// <summary>
    /// The default file name for the saveable
    /// </summary>
    public string FileName { get; } = fileName;

    /// <summary>
    /// If the filename is allowed to be overwritten by the user.
    /// Especially when creating a new instance of the saveable, since it might be forced to have a specific name.
    /// </summary>
    public bool AllowsOverwrite { get; } = allowsOverwrite;

    /// <summary>
    /// The file ending of the saveable.
    /// </summary>
    public FileTypeInformation FileType { get; } = fileType;

    /// <summary>
    /// The Path where the saveable is saved.
    /// </summary>
    public PathObj SavePath { get; } = savePath;
}