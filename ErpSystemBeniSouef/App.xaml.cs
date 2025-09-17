using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Infrastructer;
using ErpSystemBeniSouef.Infrastructer.Data;
using ErpSystemBeniSouef.Infrastructer.Data.Context;
using ErpSystemBeniSouef.Service.MainAreaServices;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.Service.SubAreaServices;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views;
using ErpSystemBeniSouef.Views.Pages.Products;
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
        services.AddScoped(typeof(IMainAreaService), typeof(MainAreaService));
        services.AddScoped(typeof(ISubAreaService), typeof(SubAreaService));
        services.AddScoped(typeof(IProductService), typeof(ProductService));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

            //using (var context = new ApplicationDbContext())
            //{ 
            //    bool count = context.mainAreas.Any();
            //    if (!context.mainAreas.Any()) // Check if the database is empty
            //                                    //لو شغال SQL Server
            //        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('mainAreas', RESEED, 0);");
            //}


            //========================================

            //using (var context = new ApplicationDbContext())
            //{
            //    bool count = context.categories.Any();
            //    if (!context.categories.Any()) // Check if the database is empty
            //                                  //لو شغال SQL Server
            //        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('categories', RESEED, 0);");
            //}


            //========================================

            //using (var context = new ApplicationDbContext())
            //{
            //    bool count = context.products.Any();
            //    if (!context.products.Any()) // Check if the database is empty
            //                                   //لو شغال SQL Server
            //        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('products', RESEED, 0);");
            //}



            //========================================

            //using (var context = new ApplicationDbContext())
            //{
            //    bool count = context.subAreas.Any();
            //    if (!context.subAreas.Any()) // Check if the database is empty
            //                                 //لو شغال SQL Server
            //        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('subAreas', RESEED, 0);");
            //}


            var mainWindowViewModel = new MainWindowViewModel();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindowViewModel.setContext(mainWindow);


            //var productService = App.AppHost.Services.GetRequiredService<IProductService>();
            var mainAreaService = App.AppHost.Services.GetRequiredService<IMainAreaService>();
            var subAreaService = App.AppHost.Services.GetRequiredService<ISubAreaService>();
            var mapper = App.AppHost.Services.GetRequiredService<IMapper>();
            //var mainRegionPage = new MainRegionPage(repo);

            //var login = new MainRegionPage(repo , mainAreaService);

            //var login = new AllProductsPage(productService, mapper);

            //var login = new MainRegionPage(mainAreaService, mapper);
            var login = new SubRegionPage(subAreaService , mapper,mainAreaService);
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
