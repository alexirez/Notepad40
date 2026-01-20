using Quicknote.Views;
using Microsoft.Extensions.DependencyInjection;


namespace Quicknote;

public partial class App : Application
{
	private readonly IServiceProvider _services;

	public App(IServiceProvider services)
	{
		InitializeComponent();
		_services = services; // store reference to service provider
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Preload settings - will add more support for settings later on
		// var settings = _services.GetRequiredService<AppSettings>();

		// Fetch mainview, which will have dependencies handled already
		var mainView = _services.GetRequiredService<MainView>();

		var window = new Window(mainView)
		{
			Title = "Quicknote",
			MinimumWidth = 430,
    		MinimumHeight = 340
		};

		return window;
	}
}