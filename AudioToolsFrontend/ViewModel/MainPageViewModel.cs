using System.Diagnostics;
using AudioTools.AudioFileTools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AudioToolsFrontend.ViewModel
{

    public partial class MainPageViewModel : ObservableObject
    {
        protected Mediator _mediator;
        protected IAudioData _audioFileData;
        public IAudioData AudioFileData
        {
            get { return _audioFileData; }
            set
            {
                if (value != null && value is IAudioData)
                {
                    _audioFileData = value;
                }
                return;
            }
        }
        public RelayCommand FilePickerCommand { get; set; }
        public RelayCommand DebugButtonCommand { get; set; }
        public MainPageViewModel(Mediator mediator)
        {
            _mediator = mediator;
            _mediator.Register<IAudioData>(HandleMyMessage);
            DebugButtonCommand = new RelayCommand(DebugButton);
            FilePickerCommand = new RelayCommand(FilePickerHandler);
        }
        private void HandleMyMessage(IAudioData message)
        {
            AudioFileData = message;
        }

        private async void DebugButton()
        {
            AudioFileData = FileHandler.HandleFileTypes("Users/collinstasisk/Documents/GitHub/AudioTools/AudioTools/TestAudioFiles");
            _mediator.Send(AudioFileData);
            Debug.WriteLine($"Sending {AudioFileData.FileName} To next view");
            await Shell.Current.GoToAsync($"{nameof(PedalBoardView)}");
        }

        private async void FilePickerHandler()
        {
            var customTypeList = new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                { DevicePlatform.WinUI, new [] { "*.mp3", "*.m4a", ".wav" } },
                { DevicePlatform.Android, new [] { "*.mp3", ".3gp", ".mp4", ".m4a", ".aac", ".ts", ".amr", ".flac", ".mid", ".xmf", ".mxmf", ".rtttl", ".rtx", ".ota", ".imy", ".mkv", ".ogg", ".wav" } },
                { DevicePlatform.iOS, new[] { "*.mp3", "*.aac", "*.aifc", "*.au", "*.aiff", "*.mp2", "*.3gp", "*.ac3" } },
                { DevicePlatform.MacCatalyst, new[] { "public.audio" } }
            };
            var customFileType = new FilePickerFileType(customTypeList);
            PickOptions options = new PickOptions
            {
                PickerTitle = "Pick Audio File to Work on",
                FileTypes = customFileType
            };
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                string fileName = result.FullPath;
                AudioFileData = FileHandler.HandleFileTypes(fileName);
                Console.WriteLine("fileName");
                //Send our data through mediator before we change views
                _mediator.Send(AudioFileData);
                await Shell.Current.GoToAsync($"{nameof(PedalBoardView)}");
                Console.WriteLine($"{AudioFileData.FileName}");
            }
            return;
        }
    }
}