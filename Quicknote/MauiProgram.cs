using Microsoft.Extensions.Logging;
using Quicknote.Views;
using Quicknote.ViewModels;
using Quicknote.Services;

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
		builder.Services.AddSingleton<FileSaver>();
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<MainView>();
		

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
