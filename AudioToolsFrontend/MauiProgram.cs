using AudioToolsFrontend.ViewModel;

namespace AudioToolsFrontend
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddTransient<PedalBoardView>();
            builder.Services.AddTransient<PedalBoardViewModel>();
            //Create our mediator that ALL viewmodels will use to communicate and ensure it only ever has one instance
            builder.Services.AddSingleton<Mediator>();

            return builder.Build();
        }
    }
}