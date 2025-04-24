using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskMaster;
using TaskMaster.Data;
using TaskMaster.ViewModels;
using TaskMaster.Views;
using TaskMaster.Services;

namespace TaskMaster;

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

        // Configuration de la base de données
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(
                "server=localhost;port=3306;user=root;password=root;database=taskmaster",
                ServerVersion.AutoDetect("server=localhost;port=3306;user=root;password=root;database=taskmaster")
            );
        });

        // Enregistrement des services
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddTransient<TasksViewModel>();
        builder.Services.AddTransient<CreateTaskViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<ProjectsViewModel>();
        builder.Services.AddTransient<ProjectDetailsViewModel>();

        // Enregistrement des pages
        builder.Services.AddTransient<TasksPage>();
        builder.Services.AddTransient<CreateTaskPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ProjectsPage>();
        builder.Services.AddTransient<ProjectDetailsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Migration de la base de données
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la migration de la base de données: {ex.Message}");
            }
        }

        return app;
    }
}

