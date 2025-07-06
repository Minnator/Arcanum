using System.Globalization;
using Arcanum.Core.CoreSystems.Common;

namespace Arcanum.Core.CoreSystems.ErrorSystem.Diagnostics;

/// <summary>
/// Represents an instance of a diagnostic which encapsulates a specific diagnostic message, description, and unique code generated from its associated descriptor.
/// </summary>
/// <remarks>
/// This class is designed to wrap a <see cref="DiagnosticDescriptor"/>, providing a formatted message, description, and code derived from the descriptor.
/// It serves as a concrete diagnostic entry that has been initialized with specific context, such as message arguments, making it usable for reporting and logging purposes.
/// </remarks>
public class Diagnostic
{
   public readonly DiagnosticDescriptor Descriptor;
   
   public DiagnosticReportSeverity ReportSeverity;
   public DiagnosticSeverity Severity;
   
   /// <summary>
   /// Formats a diagnostic message by replacing placeholders with the provided arguments, using an invariant culture.
   /// </summary>
   /// <param name="message">The message string containing placeholders to be replaced.</param>
   /// <param name="args">An array of arguments to replace the placeholders in the message string.</param>
   /// <returns>A formatted string with the placeholders replaced by the corresponding arguments or the original message if no arguments are provided.</returns>
   private static string FormatMessage(string message, params object[] args)
   {
      return args.Length == 0 ? message : string.Format(CultureInfo.InvariantCulture, message, args);
   }

   protected Diagnostic(Diagnostic diagnostic)
   {
      Descriptor = diagnostic.Descriptor;
      Message = diagnostic.Message;
      Description = diagnostic.Description;
      Code = diagnostic.Code;
   }

   public Diagnostic(DiagnosticDescriptor descriptor, params object[] args)
   {
      Descriptor = descriptor;
      Message = FormatMessage(descriptor.Message, args);
      Description = FormatMessage(descriptor.Description, args);
      Code = Descriptor
        .ToString(); // For now take the Descriptor ToString since it is equal / $"{Descriptor.Category.GetPrefix()}-{Descriptor.Id:D4}";
   }

   public virtual LocationBoundDiagnostic ToLocationBoundDiagnostic(LocationContext context)
   {
      return new(this, context);
   }

   public string Message { get; protected init; }
   public string Description;
   public string Code;
}