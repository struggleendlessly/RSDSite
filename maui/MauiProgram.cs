using Microsoft.Extensions.Logging;

using shared.Interfaces;
using shared.Managers;

namespace maui
{
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
                });

            builder.Services.AddSingleton<HttpClient>();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddTransient<AzureBlobStorageManager>();
            builder.Services.AddTransient<IFileManager, RemoteJsonFileManager>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
