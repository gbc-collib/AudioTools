using AudioTools.AudioFileTools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthKit;

namespace AudioToolsFrontend.ViewModel
{

    public partial class MainPageViewModel : ObservableObject
    {
        private IAudioData _audioFileData;
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
        public AsyncRelayCommand FilePickerCommand { get; set; }
        public MainPageViewModel()
        {
            FilePickerCommand = new AsyncRelayCommand(FilePickerHandler);
        }

        [RelayCommand]
        public async Task Button()
        {
            await Shell.Current.GoToAsync($"{nameof(PedalBoardView)}");
        }
        private async Task<FileResult> FilePickerHandler()
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
                Console.WriteLine($"{AudioFileData.FileName}");
            }
            if (result != null)
            {
                string fileName = result.FullPath;
                AudioFileData = FileHandler.HandleFileTypes(fileName);
                Console.WriteLine("fileName");
                var navigationParameter = new Dictionary<string, object>
                {
                    {"AudioObject", AudioFileData }
                };
                await Shell.Current.GoToAsync($"{nameof(PedalBoardView)}", navigationParameter);
                Console.WriteLine($"{AudioFileData.FileName}");
            }
            Console.WriteLine("Twas null");
            return result;
        }
    }
}