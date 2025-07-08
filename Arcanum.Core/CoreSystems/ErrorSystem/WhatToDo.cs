// using Arcanum.Core.CoreSystems.ErrorSystem.Diagnostics;
//
// namespace Arcanum.Core.CoreSystems.ErrorSystem;

// public class MethodState
// {
//    public DiagnosticDescriptor? Diagnostic;
//    public ErrorHandlingActions Action;
// }

/*
 * Error should be reported in the following way:
 * *Category*-*Id* *Name*(in File \"*Path*\" at Line *Row*:*Column*) (while \"*Action*\"): *message*
 * Additional Information (Description) can be shown in a popup or in another way when focusing on the error
 * The Action can be set separately of the LocationContext and should describe in what Part of the pipeline the error occurred.
 *    For Example,
 *       PA-001 Conversion Error in File \"./wrong.txt\" at Line 10:4 while \"Parsing Province ID\": Cannot convert the value of 'ashfad' to 'int'.
 *       PA-002 Duplicate Error in File \"./wrong.txt\" at Line 10:4 while \"Validating Province ID\": The Province ID '10' is duplicate and was previously defined.
 *    Alternative (Let's go with this one):
 *       PA-001 Parsing Province ID failed in File \"./wrong.txt\" at Line 10:4: Cannot convert the value of 'ashfad' to 'int'.
 *       PA-001 Parsing Country ID failed in File \"./wrong.txt\" at Line 10:4: Cannot convert the value of 'ashfad' to 'int'.
 *       PA-002 Validating Province ID failed in File \"./wrong.txt\" at Line 10:4: The Province ID '10' is duplicate and was previously defined.
 *       RO-001 Reloading Map mode failed: The border Cache was corrupted
 *    Description Example:
 *       PA-001 Conversion Error
         The value "ashfad" is not an int. Please check if either the value or the type are incorrect.

   The errors are only for the loading process

   It might be a good idea to have a debug console or log where we can send debug messages.

   How can we write methods which can be used with the loading error system and still with normal calls like UI input verification?
      => already solved this by separating the bool and the MethodState


 */
/*

 All loading helper methods should be a wrapper around the actual underlying helper method, if possible.
 If loading is by far the biggest use case, we implement the method in the loading helper method, and the default helper method warps the loading helper method.

public class Example
{

   public bool TryPareInt(string value, out int result)
   {
      return int.TryParse(value, out result);
   }

   public bool TryParseInt(string value, out int result)
   {
      return TryParseIntLoad(value, out result, out _);
   }

   // Loading case
   public bool TryParseIntLoad(string value, out int result, out Diagnostic? diagnostic)
   {
      if (TryPareInt(value, out result))
      {
         diagnostic = null;
         return true;
      }
      else
      {
         diagnostic = new DiagnosticDescriptor(
            id: 1001,
            category: DiagnosticCategory.Loading,
            severity: DiagnosticSeverity.Error,
            message: $"Could not Parse Value because:\n    PA-001 Parsing Error: Cannot convert the value of '{value}' to 'int'.",
            description: "The Parsing of the Value failed because of an error in an underlying function:\n   The value is not an int. Please check if either the value or the type are correct.",
            reportSeverity: DiagnosticReportSeverity.PopupError
         );
         result = default;
         return false;
      }
   }
}

 *
 *
 */

// value = ashfad
// but value is expected to be an int
// LO-004 Value parsing failed in File "Path" at Line 10:4": Could not Parse Value because:
//    PA-001 Parsing Error: Cannot convert the value of 'ashfad' to 'int'.
// 
/*

   *Category*-*Id* *Name*(in File \"*Path*\" at Line *Row*:*Column*) (while *Action*): *message* (because:)

 */

// LO-004 Value parsing failed : (underneath)
// LO-005 Value parsing failed : Format of the key value pair was incorrect
/*
   The Parsing of the Value failed because of an error in an underlying function:
      The value "ashfad" is not an int. Please check if either the value or the type are correct.
 */

// 
// LO-003 (Warning) Format was invalid for block name because:
//    FA-001 [Recovered] Invalid Format for xxx

// Report Method should contain the Actions
//
// [Flags]
// public enum ErrorHandlingActions
// {
//    Ignore = 0,
//    Redo = 1,
//    Skip = 2,
//    Cancel = 4,
// }
//
// public static class Test
// {
//    /// <summary>
//    /// Reports a diagnostic descriptor based on specified error-handling actions.
//    /// </summary>
//    /// <param name="actions">The error-handling actions to perform.</param>
//    /// <param name="diagnostic">The diagnostic descriptor to report.</param>
//    /// <returns>A modified set of error-handling actions after evaluating the diagnostic report.</returns>
//    public static ErrorHandlingActions Report(ErrorHandlingActions actions, DiagnosticDescriptor diagnostic)
//    {
//       // Template
//       return actions;
//    }
//
//    /// <summary>
//    /// Analyzes a failure based on the provided state and catchable actions.
//    /// </summary>
//    /// <param name="state">The current state of the method, including diagnostic information and actions.</param>
//    /// <param name="catchableActions">The set of actions that can be caught and handled.</param>
//    /// <returns>True if the failure can be analyzed and handled; otherwise, false.</returns>
//    public static bool AnalyzeFailure(MethodState state, ErrorHandlingActions catchableActions)
//    {
//       if ((state.Action & catchableActions) != 0)
//          return false;
//
//       return true;
//    }
//
//    public static bool AnalyzeAndReportFailure(MethodState state,
//                                               ErrorHandlingActions catchableActions,
//                                               ErrorHandlingActions userSelection)
//    {
//       if (state.Diagnostic is not null)
//       {
//          // Report the diagnostic
//          state.Action = Report(userSelection, state.Diagnostic);
//          state.Diagnostic = null;
//       }
//
//       if ((state.Action & catchableActions) != 0)
//          return false;
//
//       return true;
//    }
//
//    public static bool DoY(out MethodState? methodState)
//    {
//       if (DoX(out var state))
//       {
//          methodState = state;
//          // return false; // Escalate the error further
//
//          // Only Reports Failures
//          var action = Report(ErrorHandlingActions.Ignore, state!.Diagnostic!);
//
//          // Only Reacts to Actions
//          if (AnalyzeFailure(state, ErrorHandlingActions.Redo | ErrorHandlingActions.Skip))
//          {
//             methodState = state;
//             return false;
//          }
//          else
//          {
//             // Action handling Logic
//          }
//
//          // Reports Failures and Reacts to Actions
//          if (AnalyzeAndReportFailure(state,
//                                      ErrorHandlingActions.Redo | ErrorHandlingActions.Skip,
//                                      ErrorHandlingActions.Redo))
//          {
//             // User selected to redo
//             methodState = state;
//             return false; // Indicating we need to redo the operation
//          }
//          else
//          {
//             // Action handling Logic
//          }
//       }
//
//       // continue stuff
//       methodState = null;
//       return true;
//    }
//
//    public static bool DoX(out MethodState? methodState)
//    {
//       // Simulate some processing
//       methodState = new MethodState
//       {
//          Diagnostic = new DiagnosticDescriptor(id: 1001,
//                                                category: DiagnosticCategory.Loading,
//                                                severity: DiagnosticSeverity.Warning,
//                                                message: "An error occurred while processing X.",
//                                                description: "This is a simulated error for method X.",
//                                                reportSeverity: DiagnosticReportSeverity.PopupError)
//       };
//       return true; // Simulate success
//    }
// }

// how do we handle if an error occurs and the user has to interact with the reported error?
// 

// We throw an error
// We take action according to the severity of the error

// Successful / Diagnostic with DiagnosticDescriptor / ReportAction Redo / Skip 

// Method -> Loop -> Method -> Loop -> Method -> Method -> "Error"

/* if (methodState.Failure)
   {
      if(methodState.Report)
         return methodState;
      else
      {
         action = report(methodState);
         if(action != Action.Ignore)
            return action;
      }
   }

   if (methodState.Failure){
      return methodErrorEvaluator.Evaluate(methodState);
   }

   if(Eval.Eval(DoY(x), out var returnState))
      return returnState;

   if(returnState.Action == Redo)
      i -= 1;
      continue;



   if(TryCatch(DoY(x), catchableActions, out var returnState, out var actionObj))
      return returnState;
   switch()

For all
   do x
      Eval.Eval(methodState, repeatAction, out var returnState);

      do y
         return error
         for all
            do z

*/