using Arcanum.Core.Utils.vdfParser;

namespace Arcanum.Core.Globals;

public static class CoreData
{
   private static string _vanillaPath;
   public static string VanillaPath
   {
      get
      {
         if (string.IsNullOrEmpty(_vanillaPath))
            _vanillaPath = VdfParser.GetEu5Path();
         return _vanillaPath;
      }
      set => _vanillaPath = value;
   }
}