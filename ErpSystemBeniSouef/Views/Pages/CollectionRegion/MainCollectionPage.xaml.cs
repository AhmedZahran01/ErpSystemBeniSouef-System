using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Service.PettyCashServices;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.SundriesRegion;
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
    /// Interaction logic for MainCollectionPage.xaml
    /// </summary>
    public partial class MainCollectionPage : Page
    {
        private readonly IRepresentativeService _representativeService;

        public MainCollectionPage(IRepresentativeService representativeService)
        {
            InitializeComponent();
            _representativeService = representativeService;

            Loaded += async (s, e) =>
            {
                RepresentativeCombo.ItemsSource = await _representativeService.GetAllAsync();
            };

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectCollectionTab = new SelectCollectionTabPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(selectCollectionTab);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
