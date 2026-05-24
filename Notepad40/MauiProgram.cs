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
				fonts.AddFont("LibreBarcode39Extended-Regular.ttf", "LibreBarcode39");
				fonts.AddFont("LibertinusKeyboard-Regular.ttf", "LibertinusKeyboard");
				fonts.AddFont("Tiny5-Regular.ttf", "Tiny5 Regular");
				fonts.AddFont("RubikSprayPaint-Regular.ttf", "RubikSprayPaint");
				fonts.AddFont("RubikWetPaint-Regular.ttf", "RubikWetPaint Regular");
				fonts.AddFont("SixCaps-Regular.ttf", "SixCaps Regular");
				fonts.AddFont("PrincessSofia-Regular.ttf", "PrincessSofia Regular");
				fonts.AddFont("LaBelleAurore-Regular.ttf", "LaBelleAurore Regular");
				fonts.AddFont("VinaSans-Regular.ttf", "VinaSans Regular");
				fonts.AddFont("Workbench-Regular.ttf", "Workbench Regular");
				fonts.AddFont("Nosifer-Regular.ttf", "Nosifer Regular");
				fonts.AddFont("Fascinate-Regular.ttf", "Fascinate Regular");
				fonts.AddFont("GrechenFuemen-Regular.ttf", "GrechenFuemen Regular");
				fonts.AddFont("Kablammo-Regular-VariableFont_MORF.ttf", "Kablammo");
				fonts.AddFont("KolkerBrush-Regular.ttf", "KolkerBrush");
				fonts.AddFont("RibeyeMarrow-Regular.ttf", "RibeyeMarrow Regular");
				fonts.AddFont("RockSalt-Regular.ttf", "RockSalt Regular");
				fonts.AddFont("Dorsa-Regular.ttf", "Dorsa Regular");
				fonts.AddFont("Macondo-Regular.ttf", "Macondo Regular");
				fonts.AddFont("Jacquard12Charted-Regular.ttf", "Jacquard12Charted Regular");
				fonts.AddFont("Honk-Regular.ttf", "Honk Regular");
				fonts.AddFont("Ewert-Regular.ttf", "Ewert Regular");
				fonts.AddFont("ElsieSwashCaps-Black.ttf", "ElsieSwashCaps Black");
				fonts.AddFont("ElsieSwashCaps-Regular.ttf", "ElsieSwashCaps Regular");
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
