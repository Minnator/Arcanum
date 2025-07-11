using Arcanum.Core.CoreSystems.Common;

namespace Arcanum.Core.CoreSystems.SavingSystem;

public interface IFileInformationProvider
{
    public void AddSavingComment(IndentedStringBuilder sb, FileObj context);
    public void AddHeader(IndentedStringBuilder sb, FileObj context);
    public void AddFooter(IndentedStringBuilder sb, FileObj context);
    public string GetSavePrompt(FileObj context);
    
}