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

    private async void OnButtonPressed(object sender, EventArgs e)
    {
        if (sender is not VisualElement view)
            return;

        await view.ScaleToAsync(0.80, 600, Easing.CubicOut);
    }

    private async void OnButtonReleased(object sender, EventArgs e)
    {
        if (sender is not VisualElement view)
            return;

        await view.ScaleToAsync(1.0, 600, Easing.CubicOut);
    }

    void OnFontSizeCompleted(object sender, EventArgs e)
    {
        if (BindingContext is SettingsViewModel vm)
            vm.CommitFontSize();

        if (sender is Entry entry)
            entry.Unfocus();
    }

    // TODO: add auto highlight on font size focused
}