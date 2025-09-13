using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Infrastructer;
using ErpSystemBeniSouef.Infrastructer.Data;
using ErpSystemBeniSouef.Infrastructer.Data.Context;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views;
using ErpSystemBeniSouef.Views.Pages.Regions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ErpSystemBeniSouef
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //AppHost = Host.CreateDefaultBuilder()
            //        .ConfigureServices((context, services) =>
            //        {
            //            services.AddTransient<ApplicationDbContext>();
            //            services.AddScoped<IGenaricRepositoy<MainArea>, GenaricRepository<MainArea>>();
            //            services.AddScoped<IGenaricRepositoy<SubArea>, GenaricRepository<SubArea>>();
            //        })
            //        .Build();


            AppHost = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
            "Server=DESKTOP-NRGEJ6B\\SQLEXPRESS;Database=ErpSystemBeniSouef-DB;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true"
                 ));
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfwork));
    })
    .Build();

            await AppHost.StartAsync();

            try
            {
                using var scope = AppHost.Services.CreateScope();
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate(); // sync افضل هنا
                await StoreDokContextSeed.SeedAsync(dbContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // اعمل Logger مظبوط كده:
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });
                var logger = loggerFactory.CreateLogger<App>();
                logger.LogError(ex, "An error occurred during migration or seeding.");
            }
            // افتح الـ MainWindow
            var mainWindow = new Views.Windows.MainWindow();
            var mainWindowViewModel = new MainWindowViewModel();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindowViewModel.setContext(mainWindow);


            var repo = App.AppHost.Services.GetRequiredService<IUnitOfWork>();
            //var mainRegionPage = new MainRegionPage(repo);
            var login = new SubRegionPage(repo);
            //var login = new StartPageBeforeLogin();
            mainWindow.Frame.NavigationService.Navigate(login);
            mainWindow.Show();
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            if (AppHost != null)
                await AppHost.StopAsync();
            base.OnExit(e);
        }

    }
}
