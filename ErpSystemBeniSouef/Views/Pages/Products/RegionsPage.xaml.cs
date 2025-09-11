using ErpSystemBeniSouef.Core;
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
        public RegionsPage()
        {
            InitializeComponent();
        }

        private void BtnMainRegions_Click(object sender, RoutedEventArgs e)
        {
            //    var mainRegionPage = new Regions.MainRegionPage();
            //    MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(mainRegionPage);

            var repo = App.AppHost.Services.GetRequiredService<IUnitOfWork>();
            var mainRegionPage = new MainRegionPage(repo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(mainRegionPage);

        }

        private void BtnSubRegions_Click(object sender, RoutedEventArgs e)
        {
            var repo = App.AppHost.Services.GetRequiredService<IUnitOfWork>();
            var subRegionPage = new SubRegionPage(repo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(subRegionPage);

        }

        private void LeftBtn_Click(object sender, RoutedEventArgs e)
        {
         
             
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new ErpSystemBeniSouef.Views.Pages.Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

    }
}
