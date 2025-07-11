namespace Arcanum.Core.CoreSystems.SavingSystem;

public class PathObj
{
   public PathObj(string path) => Path = path.Split(['/'], StringSplitOptions.RemoveEmptyEntries);
   public static PathObj Empty { get; } = new (string.Empty);
   
   public string[] Path { get; }
   public string AbsolutPath => throw new NotImplementedException("AbsolutPath is absolutely not implemented yet.");
   public string RelativePath => throw new NotImplementedException("RelativePath is not implemented yet.");
}