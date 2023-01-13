using AudioTools.AudioFileTools;
using AudioTools.EditingTools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
            _mediator = mediator;
            _mediator.Register<IAudioData>(HandleMyMessage);
            AddPedal();
        }
        private void HandleMyMessage(IAudioData message)
        {
            AudioObject = message;
            Console.WriteLine(AudioObject.FileName);
        }
        [RelayCommand]
        public void AddPedal()
        {
            ObservableSchroederReverb BasePedalAsObservable = new(AudioObject, 150f, 0.123f, 75f);
            Debug.WriteLine("Adding Pedal");
            Pedals.Add(BasePedalAsObservable);
            SelectedPedal= BasePedalAsObservable;
            ObservableOverDrive DistasObs = new(AudioObject, 2, 1000, 500);
            Debug.WriteLine("Adding Pedal");
            Pedals.Add(DistasObs);
            SelectedPedal = DistasObs;

        }
        [RelayCommand]
        public void PrintDebug()
        {
            foreach (var pedal in Pedals)
            {
                Debug.WriteLine(pedal.GetType());
                Debug.WriteLine(pedal.Parameters["Delay"]);
                Debug.WriteLine(pedal.IsEnabled);
            }
            Debug.WriteLine($"Current Pedal is: {SelectedPedal?.GetType()}");

        }

    }
}