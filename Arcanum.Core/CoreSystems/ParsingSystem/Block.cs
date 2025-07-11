using System.Text;

namespace Arcanum.Core.CoreSystems.ParsingSystem;

public class Block(string name, int startLine, int index) : IElement
{
   public string Name { get; set; } = name;
   public bool IsBlock => true;
   public int StartLine { get; } = startLine;
   public int Index { get; } = index;

   public List<Content> ContentElements { get; set; } = [];
   public List<Block> SubBlocks { get; set; } = [];
   public int SubBlockCount => SubBlocks.Count;
   public int ContentElementCount => ContentElements.Count;
   public int Count => SubBlockCount + ContentElementCount;

   //
   // public List<Block> GetSubBlocks(bool onlyBlocks)
   // {
   //    if (onlyBlocks && ContentElements.Count > 0)
   //    {
   //       throw new ArgumentException("Expected no content elements in block: " + Name, nameof(onlyBlocks));
   //    }
   //    return SubBlocks;
   // }
   //
   // public List<Content> GetContentElements(bool onlyContent, PathObj po)
   // {
   //    if (onlyContent && SubBlocks.Count > 0)
   //    {
   //       _ = new LoadingError(po, $"Expected no subBlocks in block: {Name}!", StartLine, 0, ErrorType.UnexpectedBlockElement);
   //    }
   //    return ContentElements;
   // }
   //
   //
   //
   // public List<LineKvp<string, string>> GetContentLines(PathObj po)
   // {
   //    //Debug.Assert(SubBlockCount == 0, "This method must not be called for blocks containing other blocks!");
   //
   //    var lines = new List<LineKvp<string, string>>();
   //
   //    // We do not need to call MergeBlocksAndContent here as we only have content elements which are added in order of occurrence in the file
   //    foreach (var content in GetContentElements(true, po))
   //    {
   //       var enumerator = content.GetLineKvpEnumerator(po);
   //       foreach (var kvp in enumerator)
   //          lines.Add(kvp);
   //    }
   //
   //    return lines;
   // }
   //
   protected void AppendContent(int tabs, StringBuilder sb)
   {
      foreach (var block in SubBlocks)
         block.GetFormattedString(tabs + 1, ref sb);
      foreach (var element in ContentElements)
         element.GetFormattedString(tabs + 1, ref sb);
   }

   public string GetContent()
   {
      var sb = new StringBuilder();
      AppendContent(0, sb);
      return sb.ToString();
   }

   public string GetFormattedString(int tabs, ref StringBuilder sb)
   {
      AppendFormattedContent(tabs, ref sb);
      return sb.ToString();
   }

   public void AppendFormattedContent(int tabs, ref StringBuilder sb)
   {
      SavingTemp.OpenBlock(ref tabs, Name, ref sb);
      AppendContent(tabs, sb);
      SavingTemp.CloseBlock(ref tabs, ref sb);
   }

   // public bool GetSubBlockByName(string name, out Block block)
   // {
   //    return GetBlockByName(name, SubBlocks, out block);
   // }
   //
   // public bool GetAllSubBlockByName(string name, out List<Block> blocks)
   // {
   //    return GetAllBlockByName(name, SubBlocks, out blocks);
   // }
   //
   // /// <summary>
   // /// Returns the elements of this block in the order they appear in the file
   // /// </summary>
   // /// <returns></returns>
   // public IEnumerable<IEnhancedElement> GetElements() => EnhancedParser.MergeBlocksAndContent(SubBlocks, ContentElements);
   //
   // public static bool GetBlockByName(string name, ICollection<Block> blocks, out Block result)
   // {
   //    result = blocks.FirstOrDefault(b => b.Name.Equals(name))!;
   //    return result is not null;
   // }
   //
   // public static bool GetAllBlockByName(string name, ICollection<Block> blocks, out List<Block> result)
   // {
   //    result = blocks.Where(b => b.Name.Equals(name)).ToList()!;
   //    return result.Count > 0;
   // }
   //
   // public bool GetSubBlocksByName(string name, out List<Block> blocks)
   // {
   //    return GetBlocksByName(name, SubBlocks, out blocks);
   // }
   //
   // public static bool GetBlocksByName(string name, ICollection<Block> blocks, out List<Block> result)
   // {
   //    result = blocks.Where(b => b.Name == name).ToList();
   //    return result.Count > 0;
   // }

   public override string ToString()
   {
      return Name;
   }
}

public class Content(string value, int startLine, int index) : IElement
{
   public string Value { get; set; } = value;
   public bool IsBlock { get; } = false;
   public int StartLine { get; } = startLine;
   public int Index { get; } = index;

   public string GetContent() => Value;

   public string GetFormattedString(int tabs, ref StringBuilder sb)
   {
      AppendFormattedContent(tabs, ref sb);
      return sb.ToString();
   }

   public void AppendFormattedContent(int tabs, ref StringBuilder sb)
   {
      var enumerator = GetLineKvpEnumerator(false);
      foreach (var kvp in enumerator)
         SavingTemp.AddString(ref tabs, kvp.Value, kvp.Key, ref sb);
   }

   // public IEnumerable<(string, int)> GetLineEnumerator() 
   // {
   //    var lines = Value.Split('\n');
   //    var lineNum = StartLine;
   //    foreach (var line in lines)
   //    {
   //       if (string.IsNullOrWhiteSpace(line))
   //       {
   //          lineNum++;
   //          continue;
   //       }
   //       yield return (line, lineNum);
   //       lineNum++;
   //    }
   // }
   //
   // public IEnumerable<(string, int)> GetStringListEnumerator()
   // {
   //    var lines = Value.Split('\n');
   //    var lineNum = StartLine;
   //    foreach (var line in lines)
   //    {
   //       if (string.IsNullOrWhiteSpace(line))
   //       {
   //          lineNum++;
   //          continue;
   //       }
   //       var strings = line.Split(' ');
   //       foreach (var str in strings)
   //          yield return (str, lineNum);
   //       lineNum++;
   //    }
   // }
   //
   public IEnumerable<LineKvp<string, string>> GetLineKvpEnumerator(bool showError = true, bool trimQuotes = true)
   {
      var lines = Value.Split('\n');
      var lineNum = StartLine;
      foreach (var line in lines)
      {
         if (string.IsNullOrWhiteSpace(line))
         {
            lineNum++;
            continue;
         }

         foreach (var kvps in line.Split('\t'))
         {
            var split = kvps.Split('=');
            if (split.Length < 2)
            {
               if (showError)
                  Console.WriteLine($"Error in file x: Invalid line format at line {lineNum}: '{line}'");
               continue;
            }

            yield return new(split[0].Trim(), trimQuotes ? split[1].TrimQuotes() : split[1].Trim(), lineNum);
         }

         lineNum++;
      }
   }

   public override string ToString()
   {
      return Value;
   }
}