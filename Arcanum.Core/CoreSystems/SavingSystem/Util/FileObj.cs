namespace Arcanum.Core.CoreSystems.SavingSystem.Util;

public abstract class FileObj(PathObj path, FileDescriptor descriptor)
{
    public FileDescriptor Descriptor { get; set; } = descriptor;

    public readonly PathObj Path = path;
    
    public abstract IEnumerable<ISaveable> GetSaveables();
    public abstract Type SaveableType { get; }
}

public class FileObj<T>(PathObj path, FileDescriptor descriptor) : FileObj(path, descriptor)
    where T : ISaveable
{
    public readonly List<T> Saveables = [];

    public override IEnumerable<ISaveable> GetSaveables()
    {
        return Saveables.Cast<ISaveable>();
    }

    public override Type SaveableType => typeof(T);
}