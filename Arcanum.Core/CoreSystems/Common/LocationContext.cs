﻿using System.IO;

namespace Arcanum.Core.CoreSystems.Common;

/// <summary>
/// This context will be modified and passed around during the parsing process.
/// If an error occurs, it will be used to provide a DiagnosticDescriptor with information.
/// </summary>
public struct LocationContext
{
   
   public int LineNumber { get; set; }
   public int ColumnNumber { get; set; }
   
   public string FilePath { get; set; }
   /// <summary>
   /// The current action being performed during parsing.
   /// </summary>
   public string Action { get; set; }
   
   public override string ToString()
   {
      return $"File: {Path.GetFileName(FilePath)}, Line: {LineNumber}, Column: {ColumnNumber}, Action: {Action}";
   }
}