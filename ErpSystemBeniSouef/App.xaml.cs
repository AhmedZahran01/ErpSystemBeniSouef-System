using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.CashCustomerInvoiceServices;
using ErpSystemBeniSouef.Core.Contract.Covenant;
using ErpSystemBeniSouef.Core.Contract.CustomerInvoice;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.DueInvoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.ReturnToSupplieInvoice;
using ErpSystemBeniSouef.Core.Contract.PettyCash;
using ErpSystemBeniSouef.Core.Contract.Reports;
using ErpSystemBeniSouef.Infrastructer;
using ErpSystemBeniSouef.Infrastructer.Data;
using ErpSystemBeniSouef.Infrastructer.Data.Context;
using ErpSystemBeniSouef.Service.CashCustomerInvoices;
//using ErpSystemBeniSouef.Service.CollectionServices;
using ErpSystemBeniSouef.Service.CollectorServices;
using ErpSystemBeniSouef.Service.CovenantServices;
using ErpSystemBeniSouef.Service.CustomerInvoiceServices;
using ErpSystemBeniSouef.Service.InvoiceServices.CashInvoiceService;
using ErpSystemBeniSouef.Service.InvoiceServices.DueInvoiceService;
using ErpSystemBeniSouef.Service.InvoiceServices.ReturnSupplierInvoiceService;
using ErpSystemBeniSouef.Service.MainAreaServices;
using ErpSystemBeniSouef.Service.PettyCashServices;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.Service.ReceiptServices;
using ErpSystemBeniSouef.Service.ReportsServices;

//using ErpSystemBeniSouef.Service.ReportsServices;
using ErpSystemBeniSouef.Service.RepresentativeService;
using ErpSystemBeniSouef.Service.StoreKeeperService;
using ErpSystemBeniSouef.Service.SubAreaServices;
using ErpSystemBeniSouef.Service.SupplierAccountServices;
using ErpSystemBeniSouef.Service.supplierCashService;
using ErpSystemBeniSouef.Service.SupplierService;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.ReceiptsRegion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddScoped(typeof(ISupplierService), typeof(SupplierService));
            services.AddScoped(typeof(ICollectorService), typeof(CollectorServices));
            services.AddScoped(typeof(IRepresentativeService), typeof(RepresentativeService));
            services.AddScoped(typeof(IStoreKeeperService), typeof(StoreKeeperService));
            services.AddScoped(typeof(ICashInvoiceService), typeof(CashInvoiceService));
            services.AddScoped(typeof(ICashInvoiceItemsService), typeof(CashInvoiceItemsService));
            services.AddScoped(typeof(IDueInvoiceService), typeof(DueInvoiceService));
            services.AddScoped(typeof(IDueInvoiceItemService), typeof(DueInvoiceItemsService));
            services.AddScoped(typeof(IReturnSupplierInvoiceService), typeof(ReturnSupplierInvoiceService));
            services.AddScoped(typeof(IReturnSupplierInvoiceItemService), typeof(ReturnSupplierInvoiceItemService));
            services.AddScoped(typeof(ISupplierCashService), typeof(SupplierCashService));
            services.AddScoped(typeof(ISupplierAccountService), typeof(supplierAccountService));
            services.AddScoped(typeof(ICustomerInvoiceService), typeof(CustomerInvoiceService));
            services.AddScoped(typeof(ICashCustomerInvoiceService), typeof(CashCustomerInvoiceService));
            //services.AddScoped(typeof(ICollectionService), typeof(CollectorsReports));
            services.AddScoped(typeof(IReceiptService), typeof(ReceiptService));
            services.AddScoped(typeof(ICollectorsReports), typeof(CollectorsReports));
            services.AddScoped(typeof(IReceiptService), typeof(ReceiptService));
            services.AddScoped(typeof(ICovenantService), typeof(CovenantService));
            services.AddScoped(typeof(IPettyCashService), typeof(PettyCashService));
       
        
            //services.AddScoped(typeof(IReturnSupplierInvoiceService), typeof(ReturnSupplierInvoiceService));
            //services.AddScoped<IReturnSupplierInvoiceItemService, ReturnSupplierInvoiceItemService>();
            //services.AddScoped(typeof(IDamageInvoiceService), typeof(DamageInvoiceService));
       
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                 })    .Build();

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

            //========================================

            //using (var context = new ApplicationDbContext())
            //{
            //    bool count = context.invoiceItems.Any();
            //    if (!context.invoiceItems.Any()) // Check if the database is empty
            //                                   //لو شغال SQL Server
            //        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('invoiceItems', RESEED, 0);");
            //}

            //========================================

            var mainWindowViewModel = new MainWindowViewModel();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindowViewModel.setContext(mainWindow);


            //var productService = App.AppHost.Services.GetRequiredService<IProductService>();
            //var mainAreaService = App.AppHost.Services.GetRequiredService<IMainAreaService>();
            //var subAreaService = App.AppHost.Services.GetRequiredService<ISubAreaService>();
            //var mapper = App.AppHost.Services.GetRequiredService<IMapper>();
            //var supplierService = App.AppHost.Services.GetRequiredService<ISupplierService>();
            //var collectorService = App.AppHost.Services.GetRequiredService<ICollectorService>();
            //var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            //var storeKeeperService = App.AppHost.Services.GetRequiredService<IStoreKeeperService>();
            //var cashInvoiceService = App.AppHost.Services.GetRequiredService<ICashInvoiceService>();
            //var dueInvoiceService = App.AppHost.Services.GetRequiredService<IDueInvoiceService>();
            //var returnSupplierInvoice = App.AppHost.Services.GetRequiredService<IReturnSupplierInvoiceService>();
            //var supplierCashService = App.AppHost.Services.GetRequiredService<ISupplierCashService>();
            //var supplierAccountService = App.AppHost.Services.GetRequiredService<ISupplierAccountService>();
            //var customerInvoiceService = App.AppHost.Services.GetRequiredService<ICustomerInvoiceService>();
            //var mainRegionPage = new MainRegionPage(repo);
            var collectionService = App.AppHost.Services.GetRequiredService<ICollectorsReports>();
            var receiptService = App.AppHost.Services.GetRequiredService<IReceiptService>();


            //var login = new MainRegionPage(repo , mainAreaService);

            //var login = new AllProductsPage(productService, mapper);

            //var login = new MainRegionPage(mainAreaService, mapper);

            //var login = new SubRegionPage(subAreaService , mapper,mainAreaService);

            //var login = new SuppliersPage(supplierService);

            //var login = new CollectorPage(collectorService);

            //var login = new RepresentativePage(representativeService,mapper);

            //var login = new StorekeepersPage(storeKeeperService,mapper);

            //var login = new Views.Pages.InvoiceAndsupplierRegion.InvoicePages.
            //                   InvoicePages.Cashinvoice(supplierService, cashInvoiceService);

            //var login = new Views.Pages.InvoiceAndsupplierRegion.InvoicePages.
            //                   DueInvoice.DueInvoicePage(supplierService, dueInvoiceService);

            //var login = new Views.Pages.InvoiceAndsupplierRegion.InvoicePages.
            //                   ReturnToSupplier.ReturnToSupplieInvoicePage(supplierService, returnSupplierInvoice);

            //var login = new Views.Pages.InvoiceAndsupplierRegion.Suppliers_cash
            //                    .Suppliers_cashPage(supplierService, supplierCashService);

            //var login = new Views.Pages.InvoiceAndsupplierRegion.SupplierAccounts
            //                 .SupplierAccountsPage(supplierService,supplierCashService ,supplierAccountService);

            //var login = new Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages.Cashinvoice(0, supplierService);
            //var login = new ChooseRepresentative(representativeService);
            //var login = new Views.Pages.CustomersRegion.CustomersPage(customerInvoiceService, productService , mainAreaService
            //                          , subAreaService ,representativeService );


            //var login = new Cash_SalesPage(collectionService );
            var login = new HomeReceiptsPage();
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
