using CommunityToolkit.Maui.Extensions;
using Notepad40.ViewModels;

namespace Notepad40.Views;

public partial class MainView : ContentPage
{
    IServiceProvider _services;
    bool _isNavigating;

    public MainView(MainViewModel vm, IServiceProvider services)
    {
        InitializeComponent();
        BindingContext = vm;
        _services = services;
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
            var popup = _services.GetRequiredService<SettingsPopup>();
            await this.ShowPopupAsync(popup);
        }
        finally
        {
            _isNavigating = false;
        }
    }
}
