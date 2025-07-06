namespace Arcanum.Core.CoreSystems.SavingSystem;

public abstract class FileObj
{
   public PathObj Path;

   protected FileObj()
   {
   }

   public abstract IEnumerable<ISaveable> GetSaveables();
   public abstract string ComposeFile();
   public abstract void SaveFile();
   public abstract void LoadFile();
}

public class FileObjSingle(ISaveable saveable) : FileObj
{
   public ISaveable Saveable { get; private set; } = saveable;

   public override IEnumerable<ISaveable> GetSaveables()
   {
      throw new NotImplementedException();
   }

   public override string ComposeFile()
   {
      throw new NotImplementedException();
   }

   public override void SaveFile()
   {
      var fileData = ComposeFile();
   }

   public override void LoadFile()
   {
      throw new NotImplementedException();
   }
}