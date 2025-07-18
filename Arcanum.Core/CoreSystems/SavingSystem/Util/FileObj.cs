namespace Arcanum.Core.CoreSystems.SavingSystem.Util;

public abstract class FileObj(PathObj path, FileDescriptor descriptor, bool allowMultipleInstances)
{
    public FileDescriptor Descriptor { get; set; } = descriptor;
    
    public readonly PathObj Path = path;
    
    public readonly bool AllowMultipleInstances = allowMultipleInstances;
    
    public abstract IEnumerable<ISaveable> GetSaveables();
}

public class FileObj<T>(PathObj path, FileDescriptor descriptor, bool allowMultipleInstances) : FileObj(path, descriptor, allowMultipleInstances)
    where T : ISaveable
{
    public readonly List<T> Saveables = [];

    public override IEnumerable<ISaveable> GetSaveables()
    {
        return Saveables.Cast<ISaveable>();
    }
}