namespace Arcanum.Core.CoreSystems;

public class OhShitHereWeGoAgain_Exception : Exception
{
   public OhShitHereWeGoAgain_Exception(string message)
      : base(message)
   {
   }
   
   public OhShitHereWeGoAgain_Exception(string message, Exception innerException)
      : base(message, innerException)
   {
   }
}