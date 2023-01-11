using AudioTools.AudioFileTools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Diagnostics;

namespace AudioToolsFrontend.ViewModel
{
    public partial class PedalBoardViewModel : ObservableObject
    {
        public IAudioData AudioObject { get; set; }
        //This collection will hold dataclass that represent parameters for the sound effects and have bindable properties to control their outputs
        public ObservableCollection<object> Pedals { get; private set; }

        private object selectedPedal;
        public object SelectedPedal
        {
            get => selectedPedal;
            set
            {
                selectedPedal = value;
                OnPropertyChanged(nameof(SelectedPedal));
            }
        }
        public PedalBoardViewModel()
        {
            AddPedal();
        }
        public void AddPedal()
        {
            var newReverb = new ReverbPedal();
            Pedals.Add(newReverb);
        }
        public partial class ReverbPedal : ObservableObject
        {
            [ObservableProperty]
            private float delay;
            partial void OnDelayChanged(float value)
            {
                Debug.WriteLine(value);
            }

            [ObservableProperty]
            private float decay;

            [ObservableProperty]
            private int mixPercent;
            public ReverbPedal() 
            {
                delay = 0;
                decay = 0;
                mixPercent = 0;
            }
        }
    }
}