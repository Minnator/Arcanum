using Arcanum.Core.CoreSystems.SavingSystem.Services;

namespace Arcanum.Core.CoreSystems.SavingSystem.Util;


public class FileDescriptor(FileDescriptor[] dependencies, string[] localPath, ISavingService savingService)
{
    public readonly string[] LocalPath = localPath;
    public readonly FileDescriptor[] Dependencies = dependencies;
    public readonly ISavingService SavingService = savingService;
}