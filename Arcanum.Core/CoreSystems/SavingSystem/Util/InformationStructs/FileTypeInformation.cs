namespace Arcanum.Core.CoreSystems.SavingSystem.Util.InformationStructs;

public readonly struct FileTypeInformation(string typeName, string fileEnding, string commentPrefix)
{
    //"$\"Country file (EUV-Json)|*{.txt}\""

    public static FileTypeInformation Default = new("EUV-JSON", "txt", "#");

    public readonly string TypeName = typeName;
    public readonly string FileEnding = fileEnding;
    public readonly string CommentPrefix = commentPrefix;
}