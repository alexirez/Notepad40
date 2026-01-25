using CommunityToolkit.Maui.Extensions;
using Quicknote.ViewModels;

namespace Quicknote.Views;

public partial class MainView : ContentPage
{
    bool _isNavigating;
    public MainView(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        LoadNotes(vm); // Load notes from memory
    }

    public async void LoadNotes(MainViewModel vm)
    {
        await vm.InitializeAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainViewModel vm)
        {
            vm.OpenSettingsRequested += OnOpenSettingsRequested;
        }
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

    private async void OnOpenSettingsRequested(object? sender, EventArgs e)
    {
        if (_isNavigating)
            return;

        _isNavigating = true;

        try
        {
            await this.ShowPopupAsync(new SettingsPopup());
        }
        finally
        {
            _isNavigating = false;
        }
    }
}
