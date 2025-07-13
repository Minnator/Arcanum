using Arcanum.Core.CoreSystems.SavingSystem.Services;
using Arcanum.Core.CoreSystems.SavingSystem.Util;

namespace Arcanum.Core.CoreSystems.SavingSystem.Test;

public static class FileDescriptors
{
    public static void Test()
    {
        var provinceDefinitions = new FileDescriptor([], ["ProvinceDefinitions"], ISavingService.Dummy);
        var countryDefinitions = new FileDescriptor([provinceDefinitions], ["CountryDefinitions"], ISavingService.Dummy);
    }
}