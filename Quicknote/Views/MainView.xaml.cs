using System.Text.Json;
using Quicknote.Models;
using Quicknote.Services;
using Quicknote.ViewModels;

namespace Quicknote.Views;

public partial class MainView : ContentPage
{
    public MainView(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
