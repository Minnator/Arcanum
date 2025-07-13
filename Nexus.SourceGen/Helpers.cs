using Microsoft.CodeAnalysis;

namespace Nexus.SourceGen;

public static class Helpers
{
   public const string ExplicitPropertiesAttributeString = "Nexus.Core.ExplicitPropertiesAttribute";
   public const string IgnoreModifiableAttributeString = "Nexus.Core.IgnoreModifiableAttribute";
   public const string ModifiableAttributeString = "Nexus.Core.AddModifiableAttribute";

   public static List<ISymbol> FindModifiableMembers(INamedTypeSymbol classSymbol, bool inclusive, SourceProductionContext context)
   {
      var members = new List<ISymbol>();

      foreach (var member in classSymbol.GetMembers())
      {
         // Must not be static
         if (member.IsStatic) continue;

         // Must be a field or a property
         if (member.Kind != SymbolKind.Property && member.Kind != SymbolKind.Field) continue;

         // Must be public
         if (member.DeclaredAccessibility != Accessibility.Public) continue;

         // Must not have the [IgnoreModifiable] attribute
         var ignoreAttr = member.GetAttributes()
                                .FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == IgnoreModifiableAttributeString);

         if (ignoreAttr is not null)
         {
            if (inclusive) // not Explicit
               continue;
            var location = ignoreAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation();
            if (location != null)
            {
               // Create and report the diagnostic.
               var diagnostic = Diagnostic.Create(Diagnostics.RedundantIgnoreAttributeWarning, location);
               context.ReportDiagnostic(diagnostic);
            }

            continue;
         }

         var addAttr = member.GetAttributes()
                             .FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == ModifiableAttributeString);

         // Must have addition in Explicit mode
         if (addAttr is null)
         {
            if (!inclusive)
               continue;
         }
         else if (inclusive)
         {
            var location = addAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation();
            if (location != null)
            {
               var diagnostic = Diagnostic.Create(Diagnostics.RedundantAddAttributeWarning, location);
               context.ReportDiagnostic(diagnostic);
            }
         }

         // For properties, must have a public setter
         if (member is IPropertySymbol { SetMethod: not { DeclaredAccessibility: Accessibility.Public } })
         {
            continue;
         }

         members.Add(member);
      }

      return members;
   }
}