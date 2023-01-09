using AudioTools.AudioFileTools;

namespace AudioToolsFrontend
{
    public partial class MainPage : ContentPage
    {
        private IAudioData _audioFileData;
        public IAudioData AudioFileData 
        { 
            get { return _audioFileData; }
            set { if(value != null && _audioFileData is IAudioData)
                    {
                    _audioFileData = value;
                    Shell.Current.GoToAsync("PedalBoardView");
                    //new Command(async () => await NavigationPage.PushAsync("PedalBoardView"));


                }
                return;
                }
        }
        public MainPage(ViewModel.MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
        private async void OnFilePickerClickedAsync(object sender, EventArgs e)
        {
            var customTypeList = new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                { DevicePlatform.WinUI, new [] { "*.mp3", "*.m4a", ".wav" } },
                { DevicePlatform.Android, new [] { "*.mp3", ".3gp", ".mp4", ".m4a", ".aac", ".ts", ".amr", ".flac", ".mid", ".xmf", ".mxmf", ".rtttl", ".rtx", ".ota", ".imy", ".mkv", ".ogg", ".wav" } },
                { DevicePlatform.iOS, new[] { "*.mp3", "*.aac", "*.aifc", "*.au", "*.aiff", "*.mp2", "*.3gp", "*.ac3" } }
            };
            var customFileType = new FilePickerFileType(customTypeList);
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick Audio File to Work on",
                FileTypes = customFileType
            });
            if (result != null)
            {
                string fileName = result.FullPath;
                AudioFileData = FileHandler.HandleFileTypes(fileName);
                Console.WriteLine($"{AudioFileData.FileName}");
            }
            return;
        }
    }
}