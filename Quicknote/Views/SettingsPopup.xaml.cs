using CommunityToolkit.Maui.Views;
using Quicknote.Services;
using Quicknote.ViewModels;

namespace Quicknote.Views;

public partial class SettingsPopup : Popup
{
    public SettingsPopup(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    void OnFontSizeUnfocused(object sender, FocusEventArgs e)
{
    if (BindingContext is SettingsViewModel vm)
        vm.CommitFontSize();
}
}