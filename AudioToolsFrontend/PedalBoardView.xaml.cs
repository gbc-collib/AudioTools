using AudioToolsFrontend.ViewModel;
using AudioTools.AudioFileTools;
using AudioTools.EditingTools;
using System.Runtime.InteropServices.ObjectiveC;

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
        if (item is SchroederReverb)
        {
            return ReverbPedalTemplate;
        }
        // Other types of templates
        return null;
    }
}
