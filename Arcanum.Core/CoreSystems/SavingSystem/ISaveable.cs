namespace Arcanum.Core.CoreSystems.SavingSystem;

public readonly struct FileTypeInformation(string typeName, string fileEnding, string commentPrefix)
{
   //"$\"Country file (EUV-Json)|*{.txt}\""

   public static FileTypeInformation Default = new("EUV-JSON", "txt", "#");

   public readonly string TypeName = typeName;
   public readonly string FileEnding = fileEnding;
   public readonly string CommentPrefix = commentPrefix;
}

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

public readonly struct WriteInformation
{
}

public interface ISaveable
{
   public FileInformation GetFileInformation();
   public SaveableType SaveType { get; }
}

public class TestImplementation : ISaveable
{
   public FileInformation GetFileInformation() => throw new NotImplementedException();

   public SaveableType SaveType => SaveableType.Country;
}