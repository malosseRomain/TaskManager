using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManager.Data;
using TaskManager.ViewModels;

namespace TaskManager;

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

        // Configuration de la base de données
        builder.Services.AddDbContext<TaskDbContext>(options =>
        {
            options.UseMySql(
                "server=localhost;port=3306;database=taskmaster;user=root;password=root;",
                new MySqlServerVersion(new Version(8, 0, 30))
            )
            .EnableSensitiveDataLogging() // Affiche les données sensibles dans les journaux (à désactiver en production)
            .EnableDetailedErrors(); // Fournit des erreurs détaillées
        });

        // Enregistrement des ViewModels
        builder.Services.AddTransient<CreateTaskViewModel>();

        return builder.Build();
    }
}