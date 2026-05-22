using CommunityToolkit.Maui.Views;
using Notepad40.Services;
using Notepad40.ViewModels;

namespace Notepad40.Views;

public partial class SettingsPopup : Popup
{
    public SettingsPopup(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        CanBeDismissedByTappingOutsideOfPopup = true;
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

    private void OnCloseClicked(object sender, EventArgs e)
    {
        _ = CloseAsync();
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
                {
                    window.KeyDown += OnKeyDown;
                    Closed += (_, _) => window.KeyDown -= OnKeyDown;
                }
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

        if (e.Key == Windows.System.VirtualKey.Escape)
        {
            _ = CloseAsync();
            e.Handled = true;
        }

        if (isCtrl && e.Key == Windows.System.VirtualKey.W)
        {
            Application.Current?.CloseWindow(Application.Current.Windows[0]);
            e.Handled = true;
        }
    }
    #endif
}