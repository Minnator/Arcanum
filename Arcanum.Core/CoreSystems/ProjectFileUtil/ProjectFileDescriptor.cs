namespace Arcanum.Core.CoreSystems.ProjectFileUtil;

public class ProjectFileDescriptor
{
   public string ModName { get; }
   public string ModPath { get; }
   public bool IsSubMod { get; }
   public List<string> RequiredMods { get; } = [];
   public DateTime LastModified { get; private set; }

   public ProjectFileDescriptor(string modName, string modPath, bool isSubMod = false)
   {
      ModName = modName;
      ModPath = modPath;
      IsSubMod = isSubMod;
      LastModified = DateTime.Now;
   }

   public ProjectFileDescriptor(string modName, string modPath, bool isSubMod, List<string> requiredMods)
   {
      ModName = modName;
      ModPath = modPath;
      IsSubMod = isSubMod;
      RequiredMods = requiredMods;
      LastModified = DateTime.Now;
   }

   public void UpdateLastModified() => LastModified = DateTime.Now;

   public override string ToString()
   {
      return $"{ModName} (SubMod: {IsSubMod}, RequiredMods: {string.Join(", ", RequiredMods)})";
   }

   public override bool Equals(object? obj)
   {
      if (obj is ProjectFileDescriptor other)
      {
         return ModName == other.ModName &&
                IsSubMod == other.IsSubMod &&
                ModPath == other.ModPath &&
                RequiredMods.SequenceEqual(other.RequiredMods);
      }

      return false;
   }

   public override int GetHashCode()
   {
      var hash = new HashCode();
      hash.Add(ModName);
      hash.Add(IsSubMod);
      hash.Add(ModPath);
      foreach (var mod in RequiredMods)
         hash.Add(mod);
      return hash.ToHashCode();
   }

   public static ProjectFileDescriptor GatherFromState()
   {
      // This method should gather the necessary data from the current state of the application
      // and return a new ProjectFileDescriptor instance.
      // For now, we will return a placeholder instance.
      return new ("DefaultMod", "DefaultPath", false, ["RequiredMod1", "RequiredMod2"]);
   }
}