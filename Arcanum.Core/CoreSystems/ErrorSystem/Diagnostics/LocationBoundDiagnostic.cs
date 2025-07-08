using Arcanum.Core.CoreSystems.Common;

namespace Arcanum.Core.CoreSystems.ErrorSystem.Diagnostics;

public class LocationBoundDiagnostic
   : Diagnostic
{
   private readonly LocationContext _context;

   public LocationBoundDiagnostic(Diagnostic diagnostic, LocationContext context) : base(diagnostic)
   {
      _context = context;
      Message += $"\n{ContextToString()}";
   }

   public LocationBoundDiagnostic(DiagnosticDescriptor descriptor, LocationContext context, params object[] args) :
      base(descriptor, args)
   {
      Message = ContextToString();
      _context = context;
   }

   public override LocationBoundDiagnostic ToLocationBoundDiagnostic(LocationContext context)
   {
      throw new
         InvalidOperationException("This method should not be called on LocationBoundDiagnostic instances. Use the constructor instead.");
   }
   
   private string ContextToString()
   {
      return
         $"Error in File \"{_context.FilePath}\" at Line {_context.LineNumber:D4}, Column {_context.ColumnNumber:D3} during \"{_context.Action}\": \n\t{Message}";
   }
}