using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.PettyCash;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Service.PettyCashServices;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.Products;
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
            DateTime? CollectioneDate = ColleectionDate.SelectedDate;
            if (CollectioneDate == null)
            {
                MessageBox.Show("من فضلك اختر تاريخ صحيح");
                return;
            }

            RepresentativeDto selectedSupplier = (RepresentativeDto)RepresentativeCombo.SelectedItem;
            if (selectedSupplier == null)
            {
                MessageBox.Show("  من فضلك ادخل بيانات صحيحة واختر مندوب صحيح");
                return;
            }
             int representativeId = selectedSupplier.Id;

            var selectCollectionTab = new SelectCollectionTabPage(CollectioneDate, representativeId);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(selectCollectionTab);
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
