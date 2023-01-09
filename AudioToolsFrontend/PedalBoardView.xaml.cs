using AudioToolsFrontend.ViewModel;

namespace AudioToolsFrontend;

public partial class PedalBoardView : ContentPage
{
	public PedalBoardView(PedalBoardViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}