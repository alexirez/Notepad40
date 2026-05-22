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
				fonts.AddFont("FontdinerSwanky-Regular.ttf", "Fontdiner Swanky");
				fonts.AddFont("ShadowsIntoLight-Regular.ttf", "Shadows Into Light");
				fonts.AddFont("Audiowide-Regular.ttf", "AudioWide Regular");
				fonts.AddFont("BitcountGridDouble.ttf", "BitCountGridDouble");
				fonts.AddFont("Danfo-Regular-VariableFont_ELSH.ttf", "Danfo Regular");
				fonts.AddFont("ClickerScript-Regular.ttf", "ClickerScript Regular");
				fonts.AddFont("Smokum-Regular.ttf", "Smokum Regular");
				fonts.AddFont("CaesarDressing-Regular.ttf", "CaeserDressing Regular");
				fonts.AddFont("RubikBurned-Regular.ttf", "RubikBurned Regular");
				fonts.AddFont("Barrecito-Regular.ttf", "Barrecito Regular");
				fonts.AddFont("BungeeSpice-Regular.ttf", "BungeeSpice Regular");
				fonts.AddFont("Butcherman-Regular.ttf", "Butcherman Regular");
				fonts.AddFont("CoralPixels-Regular.ttf", "CoralPixels Regular");
				fonts.AddFont("Creepster-Regular.ttf", "Creepster Regular");
				fonts.AddFont("DiplomataSC-Regular.ttf", "DiplomataSC Regular");
				fonts.AddFont("Doto.ttf", "Doto");
				fonts.AddFont("LibreBarcode39Extended-Regular, LibreBarcode39");
				fonts.AddFont("LibertinusKeyboard-Regular", "LibertinusKeyboard");
				fonts.AddFont("Tiny5-Regular, Tiny5 Regular");
				fonts.AddFont("RubikSprayPaint-Regular", "RubikSprayPaint");
				fonts.AddFont("RubikWetPaint-Regular", "RubikWetPaint Regular");
				fonts.AddFont("SixCaps-Regular", "SixCaps Regular");
				fonts.AddFont("PrincessSofia-Regular", "PrincessSofia Regular");
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
