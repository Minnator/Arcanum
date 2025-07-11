using System.Text;

namespace Arcanum.Core.CoreSystems.ParsingSystem;

public static class ElementParser
{
   public static unsafe (List<Block>, List<Content>) GetElements(string path, string input)
   {
      var lines = input.Split('\n');
      var contents = new List<Content>();
      var blocks = new List<Block>();
      StringBuilder currentContent = new();
      ModifiableStack<Block> blockStack = new();

      var isInQuotes = false;
      var isInWord = false;
      var isInWhiteSpace = false;
      var contentStart = 0;
      var elementIndex = 0;
      byte wasEquals = 0;

      var prevWordStart = -1;
      var prevWordEnd = -1;

      for (var i = 0; i < lines.Length; i++)
      {
         var length = lines[i].Length;
         var charIndex = 0;
         var wordStart = -1;
         var wordEnd = -1;
         isInWord = false;
         isInWhiteSpace = false;

         if (lines[i].Length == 0)
         {
            currentContent.Append('\n');
            continue;
         }

         var line = lines[i].ToCharArray();

         while (charIndex < length)
         {
            var c = line[charIndex];
            switch (c)
            {
               case '\\':
                  if (isInQuotes)
                  {
                     charIndex++;
                     if (line.Length > charIndex)
                        currentContent.Append(line[charIndex]);
                  }
                  else
                     currentContent.Append(c);

                  break;
               case '"':
                  if (isInQuotes)
                     if (wasEquals == 2)
                        wasEquals = 3;
                  isInQuotes = !isInQuotes;
                  isInWhiteSpace = false;
                  currentContent.Append(c);
                  break;
               case '{':
                  if (isInQuotes)
                  {
                     currentContent.Append(c);
                     break;
                  }

                  if (currentContent.Length < 1)
                  {
                     Console.WriteLine($"Error in file {path}: Block name cannot be empty at line {i + 1}, char {charIndex + 1}");
                     return ([], []);
                  }

                  if (wordEnd < 0 || wordStart < 0)
                  {
                     if (prevWordStart < 0 || prevWordEnd < 0)
                     {
                        Console.WriteLine($"Error in file {path}: Block name cannot be empty at line {i + 1}, char {charIndex + 1}");
                        return ([], []);
                     }

                     wordStart = prevWordStart;
                     wordEnd = prevWordEnd;
                     Console.WriteLine($"Warning in file {path}: Block name is empty at line {i + 1}, char {charIndex + 1}. Using previous block name.");
                  }

                  prevWordStart = -1;
                  prevWordEnd = -1;

                  var nameLength = wordEnd - wordStart;

                  Span<char> charSpan = stackalloc char[nameLength];
                  currentContent.CopyTo(wordStart, charSpan, nameLength); // We copy the name of the block from the sb
                  currentContent.Remove(wordStart,
                                        currentContent.Length - wordStart); // we remove anything after the name start
                  Block newBlock;

                  wordStart = -1;
                  wordEnd = -1;

                  var trimmed = currentContent.ToString().Trim();

                  if (trimmed.Length >
                      0) // We have remaining content in the currentContent which we need to add to the previous block element
                  {
                     var content =
                        new Content(trimmed,
                                            contentStart,
                                            elementIndex
                                               ++); // We create a new content element as there is no block element on the stack
                     newBlock = new(new(charSpan), i, elementIndex++); // we create a new block
                     if (blockStack.IsEmpty)
                     {
                        contents.Add(content);
                        blocks.Add(newBlock);
                     }
                     else // We add the content to the previous block element
                     {
                        var currentBlock = blockStack.Peek();
                        currentBlock.ContentElements.Add(content);
                        currentBlock.SubBlocks
                                    .Add(newBlock); //TODO super rare 1 in 1000 program runs Run condition here?!
                     }
                  }
                  else // No Content to be added, only add the new Block which was started
                  {
                     newBlock = new(new(charSpan), i, elementIndex++); // we create a new block
                     if (blockStack.IsEmpty)
                        blocks.Add(newBlock);
                     else
                        blockStack.Peek().SubBlocks.Add(newBlock);
                  }

                  currentContent.Clear();
                  blockStack.Push(newBlock);
                  contentStart = i;
                  wasEquals = 0;

                  break;
               case '}':
                  if (isInQuotes)
                  {
                     currentContent.Append(c);
                     break;
                  }

                  if (blockStack.IsEmpty)
                  {
                     Console.WriteLine($"Error in file {path}: Unmatched closing brace at line {i + 1}, char {charIndex + 1}");
                     return ([], []);
                  }

                  var currentStr = currentContent.ToString();
                  var trimmedClosing = currentStr.Trim();

                  if (trimmedClosing.Length >
                      0) // We have remaining content in the currentContent which we need to add to the previous block element
                  {
                     var content = new Content(trimmedClosing,
                                                       currentStr[0] == '\n' ? contentStart + 1 : contentStart,
                                                       elementIndex
                                                          ++); // We create a new content element as there is no block element on the stack
                     blockStack.Peek().ContentElements.Add(content);
                     wordEnd = -1;
                     wordEnd = -1;
                     currentContent.Clear();
                  }

                  blockStack.Pop();
                  contentStart = i;
                  wasEquals = 0;
                  break;
               case '#':
                  if (!isInQuotes) // # is in quotes and thus allowed
                  {
                     charIndex = length;
                     break;
                  }

                  currentContent.Append(c);
                  break;
               case '\r': // WHY TF ARE THERE SINGLE \r IN THE FILES
                  if (charIndex != length - 1)
                  {
                     Console.WriteLine($"Error in file {path}: Unexpected carriage return at line {i + 1}, char {charIndex + 1}");
                     if (currentContent.Length >= 1 &&
                         char.IsWhiteSpace(currentContent[^1]) &&
                         currentContent[^1] != '\n')
                        currentContent.Remove(currentContent.Length - 1, 1);

                     currentContent.Append('\n');
                     wasEquals = 0;
                  }

                  break;
               default:
                  if (!isInQuotes) // We only add whitespace if we are in quotes
                  {
                     if (char.IsWhiteSpace(c))
                     {
                        if (!isInWhiteSpace)
                        {
                           isInWhiteSpace = true;
                           isInWord = false;
                           if (wordStart != -1)
                              if (wasEquals > 2)
                              {
                                 wasEquals = 1;
                                 currentContent.Append('\t');
                              }
                              else
                                 currentContent.Append(' ');
                        }

                        break;
                     }

                     isInWhiteSpace = false;
                     if (c != '=')
                     {
                        if (!isInWord)
                        {
                           wordEnd = currentContent.Length + 1;
                           wordStart = currentContent.Length;
                           isInWord = true;

                           if (wasEquals == 2)
                              wasEquals = 3;
                           else
                              wasEquals = 0;
                        }
                        else
                           wordEnd = currentContent.Length + 1;
                     }
                     else
                     {
                        isInWord = false;
                        wasEquals = 2;
                     }
                  }

                  currentContent.Append(c);
                  break;
            }

            charIndex++;
         }

         if (currentContent.Length >= 1 && char.IsWhiteSpace(currentContent[^1]) && currentContent[^1] != '\n')
            currentContent.Remove(currentContent.Length - 1, 1);

         if (wordStart >= 0 && wordEnd >= 0)
         {
            prevWordStart = wordStart;
            prevWordEnd = wordEnd;
         }

         currentContent.Append('\n');
         wasEquals = 0;
      }

      if (!blockStack.IsEmpty)
      {
         Console.WriteLine($"Error in file {path}: Unmatched opening brace at line {blockStack.Peek().StartLine + 1}");
         return ([], []);
      }

      if (currentContent.Length > 0)
      {
         var contentStr = currentContent.ToString();
         if (!string.IsNullOrWhiteSpace(contentStr))
            contents.Add(new(contentStr, contentStart, elementIndex++));
      }

      return (blocks, contents);
   }
}