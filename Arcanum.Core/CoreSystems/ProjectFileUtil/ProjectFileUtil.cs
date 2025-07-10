using System.IO.Compression;

namespace Arcanum.Core.CoreSystems.ProjectFileUtil;

public static class ProjectFileUtil
{
   private const string ARCANUM_PROJECT_FILE_EXTENSION = ".arcanum";

   public static string CreateFromFiles(List<string> files, string outputFileName, string outputDirectory)
   {
      if (!Directory.Exists(outputDirectory))
         // TODO: Use Ui.EnsureDirectoryExists(outputDirectory); but therefore fix template and interface for IO first
         Directory.CreateDirectory(outputDirectory);

      var outputFile = Path.Combine(outputDirectory + ARCANUM_PROJECT_FILE_EXTENSION, outputFileName);
      using var zip = ZipFile.Open(outputFile, ZipArchiveMode.Create);
      foreach (var file in files)
         zip.CreateEntryFromFile(file, Path.GetFileName(file));

      return outputFile;
   }

   public static string CreateAndRemoveEntries(List<string> files, string outputFileName, string outputDirectory)
   {
      if (!Directory.Exists(outputDirectory))
         // TODO: Use Ui.EnsureDirectoryExists(outputDirectory); but therefore fix template and interface for IO first
         Directory.CreateDirectory(outputDirectory);

      var outputFile = Path.Combine(outputDirectory + ARCANUM_PROJECT_FILE_EXTENSION, outputFileName);
      using var zip = ZipFile.Open(outputFile, ZipArchiveMode.Create);
      foreach (var file in files)
      {
         zip.CreateEntryFromFile(file, Path.GetFileName(file));
         File.Delete(file);
      }

      return outputFile;
   }

   public static string? GetFileFromProject(string projectFile, string fileName)
   {
      if (!IsValidProjectFile(projectFile))
         throw new
            InvalidDataException($"Project file '{projectFile}' is not a valid Arcanum project file. Expected extension: {ARCANUM_PROJECT_FILE_EXTENSION}");

      using var zip = ZipFile.OpenRead(projectFile);
      var entry = zip.GetEntry(fileName);
      if (entry == null)
         return null;

      var tempFilePath = Path.Combine(Path.GetTempPath(), entry.Name);
      entry.ExtractToFile(tempFilePath, true);
      return tempFilePath;
   }

   public static List<string> ExtractProjectFile(string projectFile, string outputDirectory)
   {
      if (!Directory.Exists(outputDirectory))
         // TODO: Use Ui.EnsureDirectoryExists(outputDirectory); but therefore fix template and interface for IO first
         Directory.CreateDirectory(outputDirectory);

      if (!IsValidProjectFile(projectFile))
         throw new
            InvalidDataException($"Project file '{projectFile}' is not a valid Arcanum project file. Expected extension: {ARCANUM_PROJECT_FILE_EXTENSION}");

      var extractedFiles = new List<string>();
      using var zip = ZipFile.OpenRead(projectFile);
      foreach (var entry in zip.Entries)
      {
         var filePath = Path.Combine(outputDirectory, entry.Name);
         entry.ExtractToFile(filePath, true);
         extractedFiles.Add(filePath);
      }

      return extractedFiles;
   }

   private static bool IsValidProjectFile(string projectFile)
   {
      return File.Exists(projectFile) && Path.GetExtension(projectFile) == ARCANUM_PROJECT_FILE_EXTENSION;
   }
}