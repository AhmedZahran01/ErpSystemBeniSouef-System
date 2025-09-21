using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.ViewModel;
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

namespace ErpSystemBeniSouef.Views.RepresentativeAndCollector.ReAndCoPages
{
    /// <summary>
    /// Interaction logic for SuppliersPage.xaml
    /// </summary>
    public partial class SuppliersPage : Page
    {
        #region Global Variables  Region
        
        int compId = (int)App.Current.Properties["CompanyId"];
        
        ObservableCollection<SupplierDto> observalsuppliers = new();
        IReadOnlyList<SupplierDto> supplierDtos = new List<SupplierDto>();
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;


        #endregion

        #region Constractor Region
        public SuppliersPage(ISupplierService supplierService)
        { 
            InitializeComponent(); _supplierService = supplierService;

            Loaded += async (s, e) =>
            { await LoadSuppliers(); 
            
             dgsuppliers.ItemsSource = supplierDtos;
            };
              
        }

        #endregion

        #region Load Suppliers Region

        private async Task LoadSuppliers()
        {
            supplierDtos = await _supplierService.GetAllAsync();
            observalsuppliers.Clear();
            foreach (var item in supplierDtos)
            {
                observalsuppliers.Add(item);
            }

        }

        #endregion

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var Dashboard = new MainRepresentativeAndCollectorPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }
    }
}
