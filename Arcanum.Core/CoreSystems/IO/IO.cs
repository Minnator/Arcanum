// Make sure this using is present for Bitmap

using System.Drawing.Imaging;
using System.Text;
using Arcanum.API.UtilServices;

namespace Arcanum.Core.CoreSystems.IO
{
   internal class FileOperations : IFileOperations
   {
      private readonly Encoding _windows1250Encoding;
      private readonly UTF8Encoding _bomUtf8Encoding;
      private readonly UTF8Encoding _noBomUtf8Encoding; // Standard UTF-8 (no BOM)

      public FileOperations()
      {
         Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
         _windows1250Encoding = Encoding.GetEncoding("windows-1250");
         _bomUtf8Encoding = new(true); // UTF-8 with BOM
         _noBomUtf8Encoding = new(false); // UTF-8 without BOM (same as Encoding.UTF8 default)
      }
      
      public string GetArcanumDataPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArcanumData");

      // --- Dialogs ---
      public string? SelectFolder(string startPath, string defaultFileName = "Select Folder")
      {
         EnsureDirectoryExists(startPath);

         using var dialog = new OpenFileDialog();
         dialog.InitialDirectory = startPath;
         dialog.CheckFileExists = false; // 
         dialog.CheckPathExists = true;
         dialog.FileName = defaultFileName;
         dialog.Title = "Select Folder";

         if (dialog.ShowDialog() == DialogResult.OK)
            return Path.GetDirectoryName(dialog.FileName);

         return null;
      }

      public string? SelectFile(string startFolder, string filterText)
      {
         EnsureDirectoryExists(startFolder);

         using var dialog = new OpenFileDialog();
         dialog.InitialDirectory = startFolder;
         dialog.CheckFileExists = true;
         dialog.CheckPathExists = true;
         dialog.Filter = filterText;
         dialog.Title = "Select File";

         if (dialog.ShowDialog() == DialogResult.OK)
            return dialog.FileName;

         return null;
      }

      // --- Generic File Read Operations ---
      public string? ReadAllText(string path, Encoding encoding)
      {
         if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return null;

         try
         {
            return File.ReadAllText(path, encoding);
         }
         catch (IOException)
         {
            /* TODO: Log error */
            return null;
         }
         catch (UnauthorizedAccessException)
         {
            /* TODO: Log error */
            return null;
         }
      }

      public string[]? ReadAllLines(string path, Encoding encoding)
      {
         if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return null;

         try
         {
            return File.ReadAllLines(path, encoding);
         }
         catch (IOException)
         {
            /* TODO: Log error */
            return null;
         }
         catch (UnauthorizedAccessException)
         {
            /* TODO: Log error */
            return null;
         }
      }

      // --- Specific Encoding Readers ---
      public string? ReadAllTextAnsi(string path) => ReadAllText(path, _windows1250Encoding);
      public string[]? ReadAllLinesAnsi(string path) => ReadAllLines(path, _windows1250Encoding);
      public string? ReadAllTextUtf8(string path) => ReadAllText(path, _noBomUtf8Encoding);
      public string[]? ReadAllLinesUtf8(string path) => ReadAllLines(path, _noBomUtf8Encoding);
      public string? ReadAllTextUtf8WithBom(string path) => ReadAllText(path, _bomUtf8Encoding);
      public string[]? ReadAllLinesUtf8WithBom(string path) => ReadAllLines(path, _bomUtf8Encoding);

      // --- Generic File Write Operations ---
      public bool WriteAllText(string path, string data, Encoding encoding, bool append = false)
      {
         if (string.IsNullOrEmpty(path))
            return false;

         try
         {
            if (!EnsureFileDirectoryExists(path))
               return false;

            if (append)
               File.AppendAllText(path, data, encoding);
            else
               File.WriteAllText(path, data, encoding);
            return true;
         }
         catch (IOException)
         {
            /* TODO: Log error */
            return false;
         }
         catch (UnauthorizedAccessException)
         {
            /* TODO: Log error */
            return false;
         }
      }

      // --- Specific Encoding Writers ---
      public bool WriteAllTextAnsi(string path, string data, bool append = false)
         => WriteAllText(path, data, _windows1250Encoding, append);

      public bool WriteAllTextUtf8(string path, string data, bool append = false)
         => WriteAllText(path, data, _noBomUtf8Encoding, append);

      public bool WriteAllTextUtf8WithBom(string path, string data, bool append = false)
         => WriteAllText(path, data, _bomUtf8Encoding, append);

      // --- Directory Operations ---
      public bool EnsureDirectoryExists(string directoryPath)
      {
         if (string.IsNullOrEmpty(directoryPath))
            return false;

         try
         {
            if (!Directory.Exists(directoryPath))
               Directory.CreateDirectory(directoryPath);

            return true;
         }
         catch (Exception)
         {
            /* TODO: Log error (e.g., UnauthorizedAccessException) */
            return false;
         }
      }

      private const bool ALLOW_DEFAULT_TO_APP_DIRECTORY = true;

      public bool EnsureFileDirectoryExists(string filePath)
      {
         if (string.IsNullOrEmpty(filePath))
            return false;

         var directoryPath = Path.GetDirectoryName(filePath);
         if (string.IsNullOrEmpty(directoryPath))
            // This case means filePath is just a filename like "file.txt" without a path.
            // Assuming it implies the current working directory, which should already exist.
            // Or, it could be an invalid path. For EnsureFileDirectoryExists, if there's no
            // directory part, there's nothing to ensure/create.
            return ALLOW_DEFAULT_TO_APP_DIRECTORY; // Or false, depending on desired strictness. 

         return EnsureDirectoryExists(directoryPath);
      }

      // --- File/Directory Checks ---
      public bool FileExists(string path) => !string.IsNullOrEmpty(path) && File.Exists(path);
      public bool DirectoryExists(string path) => !string.IsNullOrEmpty(path) && Directory.Exists(path);

      // --- Image Operations ---
      public bool SaveBitmap(string path, Bitmap bmp, ImageFormat format)
      {
         if (string.IsNullOrEmpty(path) || bmp == null! || format == null!)
            return false;

         try
         {
            if (!EnsureFileDirectoryExists(path))
               return false;

            bmp.Save(path, format);
            return true;
         }
         catch (Exception)
         {
            /* TODO: Log error */
            return false;
         }
      }

      // --- Async Operations ---
      public async Task<string?> ReadAllTextAsync(string path,
                                                  Encoding encoding,
                                                  CancellationToken cancellationToken = default)
      {
         if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return null;

         try
         {
            return await File.ReadAllTextAsync(path, encoding, cancellationToken);
         }
         catch (IOException)
         {
            /* TODO: Log error */
            return null;
         }
         catch (UnauthorizedAccessException)
         {
            /* TODO: Log error */
            return null;
         }
         catch (OperationCanceledException)
         {
            /* TODO: Log cancellation */
            return null;
         }
      }

      public async Task<string[]?> ReadAllLinesAsync(string path,
                                                     Encoding encoding,
                                                     CancellationToken cancellationToken = default)
      {
         if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return null;

         try
         {
            return await File.ReadAllLinesAsync(path, encoding, cancellationToken);
         }
         catch (IOException)
         {
            /* TODO: Log error */
            return null;
         }
         catch (UnauthorizedAccessException)
         {
            /* TODO: Log error */
            return null;
         }
         catch (OperationCanceledException)
         {
            /* TODO: Log cancellation */
            return null;
         }
      }

      public Task<string?> ReadAllTextAnsiAsync(string path, CancellationToken cancellationToken = default)
         => ReadAllTextAsync(path, _windows1250Encoding, cancellationToken);

      public Task<string[]?> ReadAllLinesAnsiAsync(string path, CancellationToken cancellationToken = default)
         => ReadAllLinesAsync(path, _windows1250Encoding, cancellationToken);

      public Task<string?> ReadAllTextUtf8Async(string path, CancellationToken cancellationToken = default)
         => ReadAllTextAsync(path, _noBomUtf8Encoding, cancellationToken);

      public Task<string[]?> ReadAllLinesUtf8Async(string path, CancellationToken cancellationToken = default)
         => ReadAllLinesAsync(path, _noBomUtf8Encoding, cancellationToken);

      public Task<string?> ReadAllTextUtf8WithBomAsync(string path, CancellationToken cancellationToken = default)
         => ReadAllTextAsync(path, _bomUtf8Encoding, cancellationToken);

      public Task<string[]?> ReadAllLinesUtf8WithBomAsync(string path, CancellationToken cancellationToken = default)
         => ReadAllLinesAsync(path, _bomUtf8Encoding, cancellationToken);

      public async Task<bool> WriteAllTextAsync(string path,
                                                string data,
                                                Encoding encoding,
                                                bool append = false,
                                                CancellationToken cancellationToken = default)
      {
         if (string.IsNullOrEmpty(path))
            return false;

         try
         {
            if (!EnsureFileDirectoryExists(path))
               return false;

            if (append)
               await File.AppendAllTextAsync(path, data, encoding, cancellationToken);
            else
               await File.WriteAllTextAsync(path, data, encoding, cancellationToken);
            return true;
         }
         catch (IOException)
         {
            /* TODO: Log error */
            return false;
         }
         catch (UnauthorizedAccessException)
         {
            /* TODO: Log error */
            return false;
         }
         catch (OperationCanceledException)
         {
            /* TODO: Log cancellation */
            return false;
         }
      }

      public Task<bool> WriteAllTextAnsiAsync(string path,
                                              string data,
                                              bool append = false,
                                              CancellationToken cancellationToken = default)
         => WriteAllTextAsync(path, data, _windows1250Encoding, append, cancellationToken);

      public Task<bool> WriteAllTextUtf8Async(string path,
                                              string data,
                                              bool append = false,
                                              CancellationToken cancellationToken = default)
         => WriteAllTextAsync(path, data, _noBomUtf8Encoding, append, cancellationToken);

      public Task<bool> WriteAllTextUtf8WithBomAsync(string path,
                                                     string data,
                                                     bool append = false,
                                                     CancellationToken cancellationToken = default)
         => WriteAllTextAsync(path, data, _bomUtf8Encoding, append, cancellationToken);

      public Task<bool> SaveBitmapAsync(string path,
                                        Bitmap bmp,
                                        ImageFormat format,
                                        CancellationToken cancellationToken = default)
      {
         // Bitmap.Save is synchronous. Run it on a thread pool thread.
         return Task.Run(() => SaveBitmap(path, bmp, format), cancellationToken);
      }

      public void Unload()
      {
         
      }
   }
}