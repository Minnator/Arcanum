using System.Text;

namespace Arcanum.Core.CoreSystems.ParsingSystem;

public interface IElement
{
   /// <summary>
   /// Whether this element is a block (contains sub-blocks or content elements).
   /// </summary>
   public bool IsBlock { get; }
   /// <summary>
   /// The line number where this element starts in the source file.
   /// </summary>
   public int StartLine { get; }
   /// <summary>
   /// The index of this element in the parsing order.
   /// </summary>
   public int Index { get; }

   /// <summary>
   /// Returns the content of this element as a string.
   /// </summary>
   /// <returns></returns>
   public string GetContent();

   /// <summary>
   /// Gets a formatted string representation of this element, including its content and sub-elements.
   /// </summary>
   /// <param name="tabs"></param>
   /// <param name="sb"></param>
   /// <returns></returns>
   public string GetFormattedString(int tabs, ref StringBuilder sb);

   /// <summary>
   /// Appends the formatted content of this element to the provided StringBuilder, respecting the indentation level.
   /// </summary>
   /// <param name="tabs"></param>
   /// <param name="sb"></param>
   public void AppendFormattedContent(int tabs, ref StringBuilder sb);

   public static void MergeInto(List<Block> blocks, List<Content> contents, List<IElement> output)
   {
      var ib = 0;
      var ic = 0;
      var cb = blocks.Count;
      var cc = contents.Count;

      while (ib < cb && ic < cc)
      {
         var b = blocks[ib];
         var c = contents[ic];

         if (b.Index < c.Index)
         {
            output.Add(b);
            ib++;
         }
         else
         {
            output.Add(c);
            ic++;
         }
      }

      while (ib < cb)
         output.Add(blocks[ib++]);

      while (ic < cc)
         output.Add(contents[ic++]);
   }
   
   public static IEnumerable<IElement> MergeBlocksAndContent(List<Block> blocks, List<Content> contents)
   {
      var ib = 0;
      var ic = 0;
      var cb = blocks.Count;
      var cc = contents.Count;

      while (ib < cb && ic < cc)
      {
         var b = blocks[ib];
         var c = contents[ic];

         if (b.Index < c.Index)
         {
            yield return b;

            ib++;
         }
         else
         {
            yield return c;

            ic++;
         }
      }

      while (ib < cb)
         yield return blocks[ib++];

      while (ic < cc)
         yield return contents[ic++];
   }


   public static bool operator <(IElement a, IElement b) => a.Index < b.Index;
   public static bool operator >(IElement a, IElement b) => a.Index > b.Index;
   public static bool operator <=(IElement a, IElement b) => a.Index <= b.Index;
   public static bool operator >=(IElement a, IElement b) => a.Index >= b.Index;
}