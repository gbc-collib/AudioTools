using AudioTools.AudioFileTools;
using AudioTools.EditingTools;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
namespace AudioToolsFrontend.ViewModel
{
    
    public partial class PedalBoardViewModel : ObservableObject
    {
        public IAudioData AudioObject { get; set; }
        //This collection will hold our objects that represent parameters for the sound effects and have bindable properties to control their outputs
        public ObservableCollection<IPedal> Pedals { get; private set; } = new ObservableCollection<IPedal>();
        [ObservableProperty]
        private IPedal _selectedPedal;

        private Mediator _mediator;
        public PedalBoardViewModel(Mediator mediator)
        {
            AddPedal();
            _mediator = mediator;
            _mediator.Register<IAudioData>(HandleMyMessage);
        }
        private void HandleMyMessage(IAudioData message)
        {
            AudioObject = message;
            Console.WriteLine(AudioObject.FileName);
        }
        public void AddPedal()
        {
            ObservableSchroederReverb BasePedalAsObservable = new(AudioObject, 150f, 0.123f, 75f);
            Pedals.Add(BasePedalAsObservable);
        }

    }
    [ObservableObject]
    public partial class ObservableSchroederReverb : SchroederReverb
    {
        [ObservableProperty]
        private Dictionary<string, float> _parameters;
        public ObservableSchroederReverb(IAudioData audioFile, float delay, float decay, float mixPercent) : base(audioFile, delay, decay, mixPercent)
        {
            _parameters = Parameters;
        }
    }
}