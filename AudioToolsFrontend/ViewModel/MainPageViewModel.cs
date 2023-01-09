using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace AudioToolsFrontend.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        [RelayCommand]
        async Task Button(string s)
        {
            await Shell.Current.GoToAsync(nameof(PedalBoardView));
        }
    }
    

}
