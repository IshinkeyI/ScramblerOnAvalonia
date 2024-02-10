using ReactiveUI;
using System.Reactive;
using AvaloniaApp.Models;

namespace AvaloniaApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

        private string _nonScrambledMessage;
        private string _scrambledMessage;
        private string _binaryMessage;
        private string _deScrambedMessage;
        
        private string _openWavPathMessage;
        private string _saveWavPathMessage;
        private string _scrambleKeyMessage;

        private string _scrambledOpenWavPathMessage;
        private string _scrambledSaveWavPathMessage;
        private string _scrambledWavKeyMessage;

        #region Properties

        public string NonScrambledMessage 
        { 
            get => _nonScrambledMessage;
            set => this.RaiseAndSetIfChanged(ref _nonScrambledMessage, value);
        }
        public string ScrambledMessage
        {
            get => _scrambledMessage;
            set => this.RaiseAndSetIfChanged(ref _scrambledMessage, value);
        }
        public string BinaryMessage
        {
            get => _binaryMessage;
            set => this.RaiseAndSetIfChanged(ref _binaryMessage, value);
        }
        public string DeScrambledMessage
        {
            get =>_deScrambedMessage;
            set => this.RaiseAndSetIfChanged(ref _deScrambedMessage, value);
        }
        public string OpenWavPathMessage
        {
            get => _openWavPathMessage;
            set => this.RaiseAndSetIfChanged(ref _openWavPathMessage, value);
        }
        public string SaveWavPathMessage
        {
            get => _saveWavPathMessage;
            set => this.RaiseAndSetIfChanged(ref _saveWavPathMessage, value);
        }
        public string ScrambleKeyMessage
        {
            get => _scrambleKeyMessage;
            set => this.RaiseAndSetIfChanged(ref _scrambleKeyMessage, value);
        }
        public string ScrambledOpenWavPathMessage
        {
            get => _scrambledOpenWavPathMessage;
            set => this.RaiseAndSetIfChanged(ref _scrambledOpenWavPathMessage, value);
        }
        public string ScrambledSaveWavPathMessage
        {
            get => _scrambledSaveWavPathMessage;
            set => this.RaiseAndSetIfChanged(ref _scrambledSaveWavPathMessage, value);
        }
        public string ScrambledWavKeyMessage
        {
            get => _scrambledWavKeyMessage;
            set => this.RaiseAndSetIfChanged(ref _scrambledWavKeyMessage, value);
        }

        #endregion

        #region Buttons
        public ReactiveCommand<Unit, Unit> SrambleButton { get; set; }
        public ReactiveCommand<Unit, Unit> DeSrambleButton { get; set; }
        public ReactiveCommand<Unit, Unit> OpenFileToScrambleButton { get; set; }
        public ReactiveCommand<Unit, Unit> OpenFileToDescrambleButton { get; set; }
        public ReactiveCommand<Unit, Unit> SaveScrambledFaleButton { get; set; }
        public ReactiveCommand<Unit, Unit> BinaryToMessageButton { get; set; }
        public ReactiveCommand<Unit, Unit> OpenAudioFileButton { get; set; }
        public ReactiveCommand<Unit, Unit> SaveAudioFileButton { get; set; }
        public ReactiveCommand<Unit, Unit> OpenScrambledAudioFileButton { get; set; }
        public ReactiveCommand<Unit, Unit> SaveDescrambledAudioFileButton { get; set; }
        #endregion

        private readonly ClassScrabler newStr = new();
        private readonly FileDialog fileDialog = new();

        public MainWindowViewModel()
        {
            SrambleButton = ReactiveCommand.Create(OnScrambeClick);
            DeSrambleButton = ReactiveCommand.Create(OnDeScrambleClick);
            OpenFileToScrambleButton = ReactiveCommand.Create(OnOpenToScramble);
            OpenFileToDescrambleButton = ReactiveCommand.Create(OnOpenToDescramble);
            SaveScrambledFaleButton = ReactiveCommand.Create(OnSaveScrambledClick);
            BinaryToMessageButton = ReactiveCommand.Create(OnBinaryToMessageClick);
            OpenAudioFileButton = ReactiveCommand.Create(OpenAudioFileClick);
            SaveAudioFileButton = ReactiveCommand.Create(SaveAudioFileClick);
            OpenScrambledAudioFileButton = ReactiveCommand.Create(OpenScrambledAudioFileClick);
            SaveDescrambledAudioFileButton = ReactiveCommand.Create(SaveDescrambledAudioFileClick);

            _nonScrambledMessage = "";
            _scrambledMessage = "";
            _binaryMessage = "";
            _deScrambedMessage = "";
            
            _openWavPathMessage = "";
            _saveWavPathMessage = "";
            _scrambleKeyMessage = "";

            _scrambledOpenWavPathMessage = "";
            _scrambledSaveWavPathMessage = "";
            _scrambledWavKeyMessage = "";
        }

        private void OnScrambeClick()
        {
            BinaryMessage = newStr.BinaryString(NonScrambledMessage);
            ScrambledMessage = newStr.ScrambledString(NonScrambledMessage);
        }

        private void OnDeScrambleClick()
        {
            DeScrambledMessage = newStr.MainString(ScrambledMessage, 1);
        }

        private async void OnOpenToScramble()
        {
            NonScrambledMessage = await fileDialog.OpenTextFileDialog();
        }

        private async void OnOpenToDescramble()
        {
            ScrambledMessage = await fileDialog.OpenTextFileDialog();
        }

        private void OnSaveScrambledClick()
        {
            fileDialog.SaveFileDialog(ScrambledMessage);
        }

        private void OnBinaryToMessageClick()
        {
            NonScrambledMessage = newStr.MainString(BinaryMessage, 0);
        }

        private async void OpenAudioFileClick()
        {
            OpenWavPathMessage = await fileDialog.OpenWavFileDialog(false);
        }

        private async void SaveAudioFileClick()
        {
            SaveWavPathMessage = await fileDialog.SaveWavFileDialog(ScrambleKeyMessage, false);
        }

        private async void OpenScrambledAudioFileClick()
        {
            ScrambledOpenWavPathMessage = await fileDialog.OpenWavFileDialog(true);
        }
        
        private async void SaveDescrambledAudioFileClick()
        {
            ScrambledSaveWavPathMessage = await fileDialog.SaveWavFileDialog(ScrambledWavKeyMessage, true);
        }
    }
}