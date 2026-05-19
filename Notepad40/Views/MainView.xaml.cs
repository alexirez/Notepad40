using CommunityToolkit.Maui.Extensions;
using Notepad40.ViewModels;

namespace Notepad40.Views;

public partial class MainView : ContentPage
{
    IServiceProvider _services;
    bool _isNavigating;

    const Windows.System.VirtualKey OemPlus = (Windows.System.VirtualKey)0xBB;
    const Windows.System.VirtualKey OemMinus = (Windows.System.VirtualKey)0xBD;

    private Microsoft.UI.Xaml.FrameworkElement? _keyWindow;

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

    #if WINDOWS
        // Delay slightly to ensure handlers are ready
        await Task.Delay(100);
        AttachEditorKeyboardHandlers();
    #endif
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is MainViewModel vm)
        {
            vm.OpenSettingsRequested -= OnOpenSettingsRequested;
            vm.NoteSaved -= OnNoteSaved;
        }

        #if WINDOWS
        if (_keyWindow != null)
        {
            _keyWindow.KeyDown -= OnKeyDown;
            _keyWindow = null;
        }
        #endif
    }

    private async void OnButtonPressed(object sender, EventArgs e)
    {
        if (sender is not VisualElement view)
            return;

        await view.ScaleToAsync(0.80, 300, Easing.CubicOut);
    }

    private async void OnButtonReleased(object sender, EventArgs e)
    {
        if (sender is not VisualElement view)
            return;

        await view.ScaleToAsync(1.0, 300, Easing.CubicOut);
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
        #if ANDROID || IOS
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var toast = Toast.Make("Note saved", ToastDuration.Short);
            await toast.Show();
        });
        #endif
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
                    _keyWindow = window;
                    window.KeyDown += OnKeyDown;
                }
            };
        }
    #endif
    }

    #if WINDOWS
    private void AttachEditorKeyboardHandlers()
    {
        var titleEditor = this.FindByName<Editor>("titleEditor");
        var contentEditor = this.FindByName<Editor>("contentEditor");

        if (titleEditor?.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.TextBox titleTextBox)
        {
            titleTextBox.AddHandler(
                Microsoft.UI.Xaml.UIElement.KeyDownEvent,
                new Microsoft.UI.Xaml.Input.KeyEventHandler(EditorKeyDown),
                handledEventsToo: true);
            titleTextBox.BeforeTextChanging += (s, e) => PreventZoomKeyInput(e);
        }

        if (contentEditor?.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.TextBox noteTextBox)
        {
            noteTextBox.AddHandler(
                Microsoft.UI.Xaml.UIElement.KeyDownEvent,
                new Microsoft.UI.Xaml.Input.KeyEventHandler(EditorKeyDown),
                handledEventsToo: true);
            noteTextBox.BeforeTextChanging += (s, e) => PreventZoomKeyInput(e);
        }
    }

    private void PreventZoomKeyInput(Microsoft.UI.Xaml.Controls.TextBoxBeforeTextChangingEventArgs e)
    {
        // Prevent text insertion when Ctrl+/- is pressed
        var ctrlState = Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control);
        bool isCtrl = ctrlState.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

        var isPlus = IsKeyDown(Windows.System.VirtualKey.Add) || IsKeyDown(OemPlus);
        var isMinus = IsKeyDown(Windows.System.VirtualKey.Subtract) || IsKeyDown(OemMinus);

        if (isPlus || isMinus)
            e.Cancel = true;
    }

    private async void EditorKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        var ctrlState = Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control);
        bool isCtrl = ctrlState.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

        if (isCtrl && (e.Key == Windows.System.VirtualKey.Subtract || e.Key == OemMinus))
        {
            if (BindingContext is MainViewModel vm)
                vm.DecreaseFontSizeCommand.Execute(null);
            e.Handled = true;
        }

        if (isCtrl && (e.Key == Windows.System.VirtualKey.Add || e.Key == OemPlus))
        {
            if (BindingContext is MainViewModel vm)
                vm.IncreaseFontSizeCommand.Execute(null);
            e.Handled = true;
        }

        if (isCtrl && e.Key == Windows.System.VirtualKey.S)
        {
            if (BindingContext is MainViewModel vm)
                vm.SaveNoteCommand.Execute(null);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await SaveButton.ScaleToAsync(0.80, 130, Easing.CubicOut);
                await SaveButton.ScaleToAsync(1.0, 130, Easing.CubicIn);
            });

            e.Handled = true;
        }

        if (isCtrl && e.Key == Windows.System.VirtualKey.N)
        {
            if (BindingContext is MainViewModel vm)
                vm.NewNoteCommand.Execute(null);
            
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await NewNoteButton.ScaleToAsync(0.80, 130, Easing.CubicOut);
                await NewNoteButton.ScaleToAsync(1.0, 130, Easing.CubicIn);
            });

            e.Handled = true;
        }

        if (isCtrl && e.Key == Windows.System.VirtualKey.Delete)
        {
            if (BindingContext is MainViewModel vm)
                vm.DeleteNoteCommand.Execute(null);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DeleteButton.ScaleToAsync(0.80, 130, Easing.CubicOut);
                await DeleteButton.ScaleToAsync(1.0, 130, Easing.CubicIn);
            });

            e.Handled = true;
        }
    }

    private static bool IsKeyDown(Windows.System.VirtualKey key)
    {
        return Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(key)
            .HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
    }
    #endif

    #if WINDOWS
    private async void OnKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        // Main keyboard + and - (OEM keys)
        const Windows.System.VirtualKey OemPlus = (Windows.System.VirtualKey)0xBB;
        const Windows.System.VirtualKey OemMinus = (Windows.System.VirtualKey)0xBD;

        var ctrlState = Microsoft.UI.Input.InputKeyboardSource
            .GetKeyStateForCurrentThread(Windows.System.VirtualKey.Control);
        bool isCtrl = ctrlState.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

        if (isCtrl && (e.Key == Windows.System.VirtualKey.Add || e.Key == OemPlus))
        {
            if (BindingContext is MainViewModel vm)
                vm.IncreaseFontSizeCommand.Execute(null);
            e.Handled = true;
        }

        if (isCtrl && (e.Key == Windows.System.VirtualKey.Subtract || e.Key == OemMinus))
        {
            if (BindingContext is MainViewModel vm)
                vm.DecreaseFontSizeCommand.Execute(null);
            e.Handled = true;
        }

        if (isCtrl && e.Key == Windows.System.VirtualKey.S)
        {
            if (BindingContext is MainViewModel vm)
                vm.SaveNoteCommand.Execute(null);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await SaveButton.ScaleToAsync(0.80, 130, Easing.CubicOut);
                await SaveButton.ScaleToAsync(1.0, 130, Easing.CubicIn);
            });

            e.Handled = true;
        }

        if (isCtrl && e.Key == Windows.System.VirtualKey.N)
        {
            if (BindingContext is MainViewModel vm)
                vm.NewNoteCommand.Execute(null);
            
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await NewNoteButton.ScaleToAsync(0.80, 130, Easing.CubicOut);
                await NewNoteButton.ScaleToAsync(1.0, 130, Easing.CubicIn);
            });

            e.Handled = true;
        }

        if (isCtrl && e.Key == Windows.System.VirtualKey.Delete)
        {
            if (BindingContext is MainViewModel vm)
                vm.DeleteNoteCommand.Execute(null);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DeleteButton.ScaleToAsync(0.80, 130, Easing.CubicOut);
                await DeleteButton.ScaleToAsync(1.0, 130, Easing.CubicIn);
            });

            e.Handled = true;
        }
    }
    #endif
}
