using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Covenant;
using ErpSystemBeniSouef.Core.DTOs.Covenant;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Products;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace ErpSystemBeniSouef.Views.Pages.CovenantRegion
{
    /// <summary>
    /// Interaction logic for CovenantPage.xaml
    /// </summary>
    public partial class CovenantPage : Page
    {
        private readonly IRepresentativeService _representativeService;
        private readonly ICovenantService _covenantService;
        private IReadOnlyList<RepresentativeDto> representativeDtos;
        public List<ReturnCovenant> returnCovenants = new List<ReturnCovenant>();
        public CovenantPage(IRepresentativeService representativeService, ICovenantService covenantService)
        {
            InitializeComponent();
            _representativeService = representativeService;
            _covenantService = covenantService;
            Loaded += async (s, e) =>
            {
                await loadData();
                RepresenComoBox.ItemsSource = representativeDtos;
                RepresenComoBox.SelectedIndex = 0;

                ConvenantDataGrid.ItemsSource = returnCovenants;
                RepresenComoBox.SelectedIndex = 0;
            };
        }
        public async Task loadData()
        {
            representativeDtos = await _representativeService.GetAllAsync();
            returnCovenants = await _covenantService.GetAllCovenants();

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

        #region Add Button Region
        private void AddConventBtn(object sender, RoutedEventArgs e)
        {
            DateTime? MonthDate = DateMonth.SelectedDate;
            if (MonthDate == null)
            {
                MessageBox.Show("من فضلك اختر تاريخ صحيح");
                return;
            }
            DateTime? convenentDate = ConvenentDate.SelectedDate;
            if (convenentDate == null)
            {
                MessageBox.Show("من فضلك اختر تاريخ صحيح");
                return;
            }

            RepresentativeDto selectedRep = (RepresentativeDto)RepresenComoBox.SelectedItem;
            if (selectedRep == null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }


            //string selectedtype = (string)TypeOfCovenant.SelectedItem;
            string selectedType = TypeOfCovenant.SelectedValue?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(selectedType))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            AddCovenantToRepresentative InputConvenent = new AddCovenantToRepresentative()
            {
                MonthDate = MonthDate ?? DateTime.UtcNow,
                CovenantDate = convenentDate ?? DateTime.UtcNow,
                CovenantType = selectedType,
                RepresentativeId = selectedRep.Id,
            };

            var CreateInvoiceDtoRespons = _covenantService.addCovenant(InputConvenent);
            if (!CreateInvoiceDtoRespons)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MessageBox.Show("تم إضافة العهده بنجاح");

            //RepresenComoBox.SelectedIndex = 0;
            //DateMonth.SelectedDate = null;
            //countDisplayNo++;
            //CreateInvoiceDtoRespons.DisplayId = countDisplayNo;
            //observProductsLisLim.Add(CreateInvoiceDtoRespons);
            //observProductsListFiltered.Add(CreateInvoiceDtoRespons);
        }

        #endregion

        private void DataConvenantItemsOpen(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var productService = App.AppHost.Services.GetRequiredService<IProductService>();
            var covenantService = App.AppHost.Services.GetRequiredService<ICovenantService>();
            var covenantItems = new CovenantItemsPage(productService, covenantService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(covenantItems);

        }
    }
}
