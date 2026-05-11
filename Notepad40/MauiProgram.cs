using Microsoft.Extensions.Logging;
using Notepad40.Views;
using Notepad40.ViewModels;
using Notepad40.Services;
using Notepad40.Models;
using CommunityToolkit.Maui;

namespace Notepad40;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// ------------------------
		// Add dependencies
		// ------------------------
		builder.UseMauiCommunityToolkit();

		// ------------------------
        // Register services
        // ------------------------
		builder.Services.AddSingleton<AppSettings>();
		builder.Services.AddSingleton<ISettingsManager, SettingsManager>();
		builder.Services.AddSingleton<INoteService, NoteService>();
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<MainView>();
		builder.Services.AddTransient<SettingsPopup>();
		builder.Services.AddTransient<SettingsViewModel>();
		

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
