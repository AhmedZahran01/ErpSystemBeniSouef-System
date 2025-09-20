using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.Service.MainAreaServices;
using ErpSystemBeniSouef.Service.ProductService;
using ErpSystemBeniSouef.Service.SupplierService;
using ErpSystemBeniSouef.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages
{
    /// <summary>
    /// Interaction logic for Cashinvoice.xaml
    /// </summary>
    public partial class Cashinvoice : Page
    {

        #region Global Variables  Region
        private readonly int _companyNo;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper; 
 
        IReadOnlyList<SupplierDto> SuppliersDto = new List<SupplierDto>();

        #endregion
        //List<Suppliers> SupplierNames = new List<Suppliers>();
        //List<CashInvoice> CashInvoiceData = new List<CashInvoice>();
        public Cashinvoice(int companyNo , ISupplierService supplierService)
        {
            InitializeComponent();
            _companyNo = companyNo;
            _supplierService = supplierService;

            Loaded += async (s, e) =>
            { 
                cb_SuppliersName.ItemsSource = await _supplierService.GetAllAsync(); ;
                //cb_type.ItemsSource = await _productService.GetAllCategoriesAsync();
                cb_SuppliersName.SelectedIndex = 0;
            };
 
        }
          
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        { 
            var invoicesRegion = new Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicesRegion(_companyNo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicesRegion);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
