using ErpSystemBeniSouef.Core.Contract;
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

namespace ErpSystemBeniSouef.Views.Pages.RepresenWithdrawalRegion
{
    /// <summary>
    /// Interaction logic for RepresenWithdrawalPage.xaml
    /// </summary>
    public partial class RepresenWithdrawalPage : Page
    {
        private readonly IRepresentativeService _representativeService;

        public RepresenWithdrawalPage(IRepresentativeService representativeService)
        {
            InitializeComponent();
            _representativeService = representativeService;
            Loaded += async (s, e) =>
            {
                RepresentativeCombo.ItemsSource = await _representativeService.GetAllAsync();
            };
        }

        private void AddRepresenWithdrawalBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteWithdrawalBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Dahbord Back Button Click Region
        private void DahbordBackButton_Click(object sender, RoutedEventArgs e)
        {
            var invoicesRegion = new Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicesRegion);
        }


        #endregion

        private void DecimalOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
