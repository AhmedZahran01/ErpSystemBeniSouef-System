using AutoMapper;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Regions;
using ErpSystemBeniSouef.Views.Windows;
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

namespace ErpSystemBeniSouef.Views.Pages.Products
{
    /// <summary>
    /// Interaction logic for RegionsPage.xaml
    /// </summary>
    public partial class RegionsPage : Page
    {
        #region Constractor Region
       
        public RegionsPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Btn Main Regions Region

        private void BtnMainRegions_Click(object sender, RoutedEventArgs e)
        {
            //    var mainRegionPage = new Regions.MainRegionPage();
            //    MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(mainRegionPage);

            var mainAreaServiceRepo = App.AppHost.Services.GetRequiredService<IMainAreaService>();
            var mapper = App.AppHost.Services.GetRequiredService<IMapper>();
            var mainRegionPage = new MainRegionPage(mainAreaServiceRepo, mapper);
            //var mainRegionPage = new MainRegionPage(repo, mainAreaServiceRepo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(mainRegionPage);

        }

        #endregion

        #region Btn Sub Regions Region

        private void BtnSubRegions_Click(object sender, RoutedEventArgs e)
        {
            var subAreaService = App.AppHost.Services.GetRequiredService<ISubAreaService>();
            var mainAreaService = App.AppHost.Services.GetRequiredService<IMainAreaService>();
            var mapper = App.AppHost.Services.GetRequiredService<IMapper>();
            var subRegionPage = new SubRegionPage(subAreaService, mapper, mainAreaService);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(subRegionPage);

        }

        #endregion

        #region Left Btn Region

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {


        }

        #endregion

        #region Back Btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new ErpSystemBeniSouef.Views.Pages.Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

        #endregion

    }
}
