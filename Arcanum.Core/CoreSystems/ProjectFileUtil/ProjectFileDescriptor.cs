namespace Arcanum.Core.CoreSystems.ProjectFileUtil;

public class ProjectFileDescriptor
{
   public string ModeName { get; }
   public bool IsSubMOd { get; } = false;
   public List<string> RequiredMods { get; } = [];
   public DateTime LastModified { get; set; }
   
   public ProjectFileDescriptor(string modeName, bool isSubMod = false)
   {
      ModeName = modeName;
      IsSubMOd = isSubMod;
      LastModified = DateTime.Now;
   }
   
   public ProjectFileDescriptor(string modeName, bool isSubMod, List<string> requiredMods)
   {
      ModeName = modeName;
      IsSubMOd = isSubMod;
      RequiredMods = requiredMods;
      LastModified = DateTime.Now;
   }
   
   public void UpdateLastModified() => LastModified = DateTime.Now;

   public override string ToString()
   {
      return $"{ModeName} (SubMod: {IsSubMOd}, RequiredMods: {string.Join(", ", RequiredMods)})";
   }
   
   public override bool Equals(object? obj)
   {
      if (obj is ProjectFileDescriptor other)
      {
         return ModeName == other.ModeName &&
                IsSubMOd == other.IsSubMOd &&
                RequiredMods.SequenceEqual(other.RequiredMods);
      }
      return false;
   }
   
   public override int GetHashCode()
   {
      var hash = new HashCode();
      hash.Add(ModeName);
      hash.Add(IsSubMOd);
      foreach (var mod in RequiredMods)
         hash.Add(mod);
      return hash.ToHashCode();
   }

}