using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Reports;
using ErpSystemBeniSouef.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ClosedXML.Excel.XLPredefinedFormat;
using DateTime = System.DateTime;

namespace ErpSystemBeniSouef.Views.Pages.ReportsRegion.ReportsPages
{
    /// <summary>
    /// Interaction logic for Cash_SalesPage.xaml
    /// </summary>
    public partial class Cash_SalesPage : Page
    {
        private readonly ICollectionService _collectionService;
        private readonly System.DateTime dateTime1;
        private readonly System.DateTime dateTime2;
        private readonly int repId;
        public Cash_SalesPage( )
        //public Cash_SalesPage(ICollectionService collectionService, DateTime dateTime1, DateTime dateTime2, int repId)
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                try
                {
                    await LoadData();
                }
                catch
                {

                }

            };
        }

        private async Task LoadData()
        {
            var s = await _collectionService.GetRepresentativeCashInvoicesAsync(dateTime1, dateTime2, repId);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var customersPage = new ChooseRepresentative(representativeService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(customersPage);
        }
    }
}
