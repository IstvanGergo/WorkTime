using Microsoft.Extensions.Logging;
using WorkTime.View;

namespace WorkTime;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<WorkData>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<WorkTimeMainViewModel>();
		builder.Services.AddSingleton<AddItemsPage>();
        builder.Services.AddSingleton<AddItemsPageViewModel>();

        return builder.Build();
	}
}
