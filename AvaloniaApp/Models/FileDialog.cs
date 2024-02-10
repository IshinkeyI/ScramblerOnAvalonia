using System.IO;
using System.Text;
using Avalonia.Controls;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace AvaloniaApp.Models
{
    public class FileDialog
    {
        private EncryptFile encryptFile;

        private string openWavString;
        private string saveWavString;

        private string scrambledOpenWavString;
        private string scrambledSaveWavString;

        public FileDialog()
        {
            encryptFile = new EncryptFile();
            openWavString = "";
            saveWavString = "";
            scrambledOpenWavString = "";
            scrambledSaveWavString = "";
        }

        public async Task<string> OpenTextFileDialog()
        {
            var window = new Window();
            string readToEnd = string.Empty;
            var files = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open File",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Text")      { Patterns = new[] { "*.txt" } },
                    new FilePickerFileType("All Files") { Patterns = new[] { "*" } }
                }
            });

            if (files.Count >= 1)
            {
                // Open reading stream from the first file.
                await using var stream = await files[0].OpenReadAsync();
                using var streamReader = new StreamReader(stream, Encoding.Default);
                // Reads all the content of file as a text.
                readToEnd = await streamReader.ReadToEndAsync();
            }
            return readToEnd;
        }

        public async Task<string> OpenWavFileDialog(bool isScrambled)
        {
            var window = new Window();
            var files = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open File",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("WAV")      { Patterns = new[] { "*.wav" } },
                    new FilePickerFileType("All Files") { Patterns = new[] { "*" } }
                }
            });
            
            if(files.Count >= 1)
            {
                if (isScrambled)
                {
                    scrambledOpenWavString = files[0].Path.ToString().Replace("file:///", "");
                    return scrambledOpenWavString;
                }
                else
                {
                    openWavString = files[0].Path.ToString().Replace("file:///", "");
                    return openWavString;
                }
            }

            return string.Empty;
        }

        public async void SaveFileDialog(string scrambledMessage)
        {
            var window = new Window();

            // Start async operation to open the dialog.
            var file = await window.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save File", 
                FileTypeChoices = new[] 
                {
                    new FilePickerFileType("Text") { Patterns = new[] { "*.txt" } } 
                }
            });

            if (file is not null)
            {
                // Open writing stream from the file.
                await using var stream = await file.OpenWriteAsync();
                using var streamWriter = new StreamWriter(stream, Encoding.Default);
                // Write some content to the file.
                await streamWriter.WriteAsync(scrambledMessage);
            }
        }

        public async Task<string> SaveWavFileDialog(string key, bool isScrambled)
        {
            var window = new Window();

            // Start async operation to open the dialog.
            var file = await window.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save File",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("WAV") { Patterns = new[] { "*.wav" } }
                }
            });

            if (file is not null)
            {
                if (isScrambled)
                {
                    scrambledSaveWavString = file.Path.ToString().Replace("file:///", "");
                    encryptFile.Decrypt(scrambledOpenWavString, scrambledSaveWavString, key);
                    return scrambledSaveWavString;
                }
                else
                {

                    saveWavString = file.Path.ToString().Replace("file:///", "");
                    encryptFile.Encrypt(openWavString, saveWavString, key);
                    return saveWavString;
                }
            }
            return string.Empty;
        }

    }
}
