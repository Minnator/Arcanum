using System.Text;

namespace Arcanum.Core.CoreSystems.ParsingSystem;

public interface IElement
{
   public bool IsBlock { get; }
   public int StartLine { get; }
   public int Index { get; }
   public string GetContent();
   public string GetFormattedString(int tabs, ref StringBuilder sb);
   public void AppendFormattedContent(int tabs, ref StringBuilder sb);

   public static bool operator < (IElement a, IElement b) => a.Index < b.Index;
   public static bool operator > (IElement a, IElement b) => a.Index > b.Index;
   public static bool operator <= (IElement a, IElement b) => a.Index <= b.Index;
   public static bool operator >= (IElement a, IElement b) => a.Index >= b.Index;
}