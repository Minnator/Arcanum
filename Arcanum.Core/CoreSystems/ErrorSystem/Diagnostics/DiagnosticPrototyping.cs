using ErrorProvider = Arcanum.Core.CoreSystems.ErrorSystem.BaseErrorTypes.ErrorProvider;

namespace Arcanum.Core.CoreSystems.ErrorSystem.Diagnostics;

public static class DiagnosticPrototyping
{
   public static bool ConvertToInt(string toConvert, out int value, out Diagnostic? diagnostic)
   {
      diagnostic = null;
      if (int.TryParse(toConvert, out value))
         return true;

      diagnostic = new(
                       ErrorProvider.ConversionED,
                       toConvert,
                       typeof(int)
                      );
      return false;
   }


   public static Diagnostic? Failure()
   {
      var testString = "HEHE";
      ConvertToInt(testString, out _, out var diagnostic);
      //diagnostic = diagnostic.Encapsulate(Diagnostic)
      return diagnostic;
   }
   
   public static Diagnostic? Success()
   {
      var testString = "123";
      ConvertToInt(testString, out _, out var diagnostic);
      return diagnostic;
   }
}