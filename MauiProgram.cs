
using PracticalWork2Calculator.Services;
using PracticalWork2Calculator.Views;
using Microsoft.Extensions.Logging;

namespace PracticalWork2Calculator;

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

        
        builder.Services.AddSingleton<UserManager>();

        
        builder.Services.AddSingleton<LoginPage>(); 
        builder.Services.AddTransient<RegisterPage>(); 
        builder.Services.AddTransient<PasswordRecoveryPage>();
        builder.Services.AddTransient<ConverterPage>();
        builder.Services.AddTransient<UserInfoPage>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}