using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
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

namespace ErpSystemBeniSouef.Views.Pages.ReportsRegion
{
    /// <summary>
    /// Interaction logic for ChooseRepresentative.xaml
    /// </summary>
    public partial class ChooseRepresentative : Page
    {
        private readonly IRepresentativeService _representativeService;

        public ChooseRepresentative(IRepresentativeService representativeService)
        {
            InitializeComponent();
            _representativeService = representativeService;
            Loaded += async (s, e) =>
            {
                RepresentativeCombo.ItemsSource = await _representativeService.GetAllAsync();
                RepresentativeCombo.SelectedIndex = 0;
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDatePicker = StartDatePicker.SelectedDate;
            DateTime? endDatePicker = EndDatePicker.SelectedDate;

            if (startDatePicker == null)
            {
                MessageBox.Show("يرجى تحديد تاريخ بدايه الحساب");
                return;
            }
            if (endDatePicker == null)
            {
                MessageBox.Show("يرجى تحديد تاريخ نهايه الحساب");
                return;
            }

            int RepresentativeId = ((RepresentativeDto)RepresentativeCombo.SelectedItem).Id;
            if (RepresentativeId <= 0)
            {
                MessageBox.Show(" اخيار المندوب خاطي ");
                return;
            } 
            var pr = new HomeReportsChooseTab(startDatePicker??DateTime.UtcNow , endDatePicker ?? DateTime.UtcNow, RepresentativeId);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(pr);

        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new Products.Dashboard();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }
    }
}
