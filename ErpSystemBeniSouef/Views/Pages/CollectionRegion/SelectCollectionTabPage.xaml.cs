using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Service.PettyCashServices;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.SundriesRegion;
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

namespace ErpSystemBeniSouef.Views.Pages.CollectionRegion
{
    /// <summary>
    /// Interaction logic for SelectCollectionTabPage.xaml
    /// </summary>
    public partial class SelectCollectionTabPage : Page
    {
        public SelectCollectionTabPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var representativeService = App.AppHost.Services.GetRequiredService<IRepresentativeService>();
            var monthlyMoney = new MonthlyMoneyCollection();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(monthlyMoney);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
