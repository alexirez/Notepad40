using Microsoft.Extensions.Logging;
using Quicknote.Views;
using Quicknote.ViewModels;
using Quicknote.Services;
using Quicknote.Models;

namespace Quicknote;

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
        // Register services
        // ------------------------
		builder.Services.AddSingleton<AppSettings>();
		builder.Services.AddSingleton<ISettingsManager, SettingsManager>();
		builder.Services.AddSingleton<INoteService, NoteService>();
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<MainView>();
		

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
