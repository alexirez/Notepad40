using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
            vm.NoteSaved += OnNoteSaved;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is MainViewModel vm)
        {
            vm.OpenSettingsRequested -= OnOpenSettingsRequested;
            vm.NoteSaved -= OnNoteSaved;
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

    /*An event to show a snackbar when saving*/
    private async void OnNoteSaved(object? sender, EventArgs e)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var toast = Toast.Make("Note saved", ToastDuration.Short);
            await toast.Show();
        });
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

    #if WINDOWS
        var view = Handler?.PlatformView as Microsoft.UI.Xaml.FrameworkElement;
        if (view != null)
        {
            view.Loaded += (s, e) =>
            {
                var window = view.XamlRoot?.Content as Microsoft.UI.Xaml.FrameworkElement;
                if (window != null)
                    window.KeyDown += OnKeyDown;
            };
        }
    #endif
    }

    #if WINDOWS
    private void OnKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        var ctrlState = Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control);
        bool isCtrl = ctrlState.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

        if (isCtrl && e.Key == Windows.System.VirtualKey.S)
        {
            if (BindingContext is MainViewModel vm)
                vm.SaveNoteCommand.Execute(null);
            e.Handled = true;
        }
    }
    #endif
}
