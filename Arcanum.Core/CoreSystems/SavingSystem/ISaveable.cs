using Arcanum.Core.CoreSystems.Common;

namespace Arcanum.Core.CoreSystems.SavingSystem;

public readonly struct FileInformation(string fileName, string fileEnding, PathObj saveType, bool allowsOverwrite)
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
   public string FileEnding { get; } = fileEnding;
   
   /// <summary>
   /// The Path where the saveable is saved.
   /// </summary>
   public PathObj SavePath { get; } = saveType;
}

public readonly struct WriteInformation{
   
}

public interface ISaveable
{
   public SaveableType SaveType { get; }

   public FileInformation GetFileInformation();

   public void AddHeader(IndentedStringBuilder builder)
   {

   }
}

public class TestImplementation: ISaveable
{
   public SaveableType SaveType => SaveableType.Country;
   public FileInformation GetFileInformation() => throw new NotImplementedException();

}