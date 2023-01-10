using AudioTools.AudioFileTools;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AudioToolsFrontend.ViewModel
{
    public partial class PedalBoardViewModel : ObservableObject
    {
        public IAudioData AudioObject { get; set; }
    }
}
