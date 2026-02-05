using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.CollectionDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Service.CollectionServices;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Products;
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

namespace ErpSystemBeniSouef.Views.Pages.CollectionRegion
{
    /// <summary>
    /// Interaction logic for MonthlyMoneyCollection.xaml
    /// </summary>
    public partial class MonthlyMoneyCollection : Page
    {
        private readonly ICollectionService collectionService;

        private readonly DateTime? collectioneDate;
        private readonly int representativeId;
        //List<CollectorInstallmentsResultDto> dataR = new List<CollectorInstallmentsResultDto>();
        CollectorInstallmentsResultDto AllData = new CollectorInstallmentsResultDto();


        public MonthlyMoneyCollection(ICollectionService collectionService, DateTime? CollectioneDate, int representativeId)
        {
            InitializeComponent();
            this.collectionService = collectionService;

            collectioneDate = CollectioneDate;
            this.representativeId = representativeId;

            Loaded += async (s, e) =>
            {
                await LoadcollectionService();

                collectionServiceGrids.ItemsSource = AllData.Installments;
            };


        }


        #region Load representative Withdrawal to Grid Region

        private async Task LoadcollectionService()
        {
            AllData = await collectionService.GetCollectorInstallmentsAsync(representativeId, collectioneDate ?? DateTime.UtcNow);


        }

        #endregion


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Dahbord Back Button Click Region
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var DashbordRegion = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(DashbordRegion);
        }

        #endregion

    }
}
