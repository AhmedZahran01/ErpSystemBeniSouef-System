using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.RepresentativeWithdrawalDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Service.PettyCashServices;
using ErpSystemBeniSouef.Service.RepresentativeService;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Products;
using ErpSystemBeniSouef.Views.Pages.SundriesRegion;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SelectCollectionTabPage.xaml
    /// </summary>
    public partial class SelectCollectionTabPage : Page
    {
        private readonly DateTime? collectioneDate;
        private readonly int representativeId;

        public SelectCollectionTabPage(DateTime? CollectioneDate , int representativeId)
        {
            InitializeComponent();
            collectioneDate = CollectioneDate;
            this.representativeId = representativeId;

             
        }


       


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var collectionService = App.AppHost.Services.GetRequiredService<ICollectionService>();
            var monthlyMoney = new MonthlyMoneyCollection(collectionService ,collectioneDate, representativeId);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(monthlyMoney);
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
