using Arcanum.Core.CoreSystems.SavingSystem.Services;
using Arcanum.Core.CoreSystems.SavingSystem.Util.InformationStructs;

namespace Arcanum.Core.CoreSystems.SavingSystem.Util;


public class FileDescriptor(
    FileDescriptor[] dependencies,
    string[] localPath,
    ISavingService savingService,
    FileTypeInformation fileType)
{
    public readonly string[] LocalPath = localPath;
    public readonly FileDescriptor[] Dependencies = dependencies;
    public readonly ISavingService SavingService = savingService;
    public FileTypeInformation FileType = fileType;

    public List<FileObj> Files = [];
    
    public string GetFilePath()
    {
        return $"{string.Join("/", LocalPath)}";
    }
}