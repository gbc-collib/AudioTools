using AudioToolsFrontend.ViewModel;
using AudioTools.EditingTools;
using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace AudioToolsFrontend;

public partial class PedalBoardView : ContentPage
{
    public PedalBoardView(PedalBoardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
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
        var viewCell = new ViewCell();

        if (item is SchroederReverb)
        {
            return SchroederReverbPedalTemplate;
        }
        else if (item is OverDriveDistortion)
        {
            viewCell.BindingContext = item;
            return OverDrivePedalTemplate;
        }

        return null;
    }
}

