using AudioTools.AudioFileTools;
using AudioToolsFrontend.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AudioToolsFrontend;

public partial class PedalBoardView : ContentPage
{
	public PedalBoardView(PedalBoardViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
public class PedalDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate ReverbPedalTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is PedalBoardViewModel.ReverbPedal)
        {
            return ReverbPedalTemplate;
        }
        // Other types of templates
        return null;
    }
}
