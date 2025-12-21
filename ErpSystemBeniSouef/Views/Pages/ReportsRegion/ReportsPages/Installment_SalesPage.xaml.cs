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

namespace ErpSystemBeniSouef.Views.Pages.ReportsRegion.ReportsPages
{
    /// <summary>
    /// Interaction logic for Installment_SalesPage.xaml
    /// </summary>
    public partial class Installment_SalesPage : Page
    {
        private readonly ICollectionService _collectionService;
        private readonly DateTime dateTime1;
        private readonly DateTime dateTime2;
        private readonly int repId;

        public Installment_SalesPage(ICollectionService collectionService, DateTime dateTime1, DateTime dateTime2, int repId)
        {
            InitializeComponent();
            _collectionService = collectionService;
            this.dateTime1 = dateTime1;
            this.dateTime2 = dateTime2;
            this.repId = repId;
            //Loaded += async (s, d) =>
            //{
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async Task LoadData()
        {
            var s = await _collectionService.GetRepresentativeCovenantsAsync(dateTime1, dateTime2, repId);

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var customersPage = new ChooseRepresentative(representativeService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(customersPage);

        }
    }
}
