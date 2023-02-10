using AudioToolsFrontend.ViewModel;
using AudioTools.EditingTools;

namespace AudioToolsFrontend;

public partial class PedalBoardView : ContentPage
{
    public PedalBoardView(PedalBoardViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }

    private void mixSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {

    }
}
public class PedalDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate SchroederReverbPedalTemplate { get; set; }
    public DataTemplate OverDrivePedalTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {

        if (item is SchroederReverb)
        {
            return SchroederReverbPedalTemplate;
        }
        else if (item is OverDriveDistortion)
        {
            return OverDrivePedalTemplate;
        }

        return null;
    }
}

