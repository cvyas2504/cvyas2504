using FrontOfficeApp.Data;
using FrontOfficeApp.Services;
using FrontOfficeApp.ViewModels;
using FrontOfficeApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrontOfficeApp;

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

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "frontoffice.db");
        builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlite($"Data Source={dbPath}"));

        builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
        builder.Services.AddSingleton<ISessionService, SessionService>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IDutyRosterService, DutyRosterService>();
        builder.Services.AddScoped<IExcelComparisonService, ExcelComparisonService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped<IDataSeeder, DataSeeder>();
        builder.Services.AddSingleton<IApiClient, ApiClient>();

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<ExcelComparePage>();
        builder.Services.AddTransient<EmployeesPage>();
        builder.Services.AddTransient<DutyRosterPage>();
        builder.Services.AddTransient<ReportsPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<UserManagementPage>();
        builder.Services.AddTransient<AboutPage>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<ExcelCompareViewModel>();
        builder.Services.AddTransient<EmployeesViewModel>();
        builder.Services.AddTransient<DutyRosterViewModel>();
        builder.Services.AddTransient<ReportsViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<UserManagementViewModel>();
        builder.Services.AddTransient<AboutViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        seeder.SeedAsync().GetAwaiter().GetResult();

        return app;
    }
}
