using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.DTOs.ProductDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
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
        private readonly int _companyNo = 1;
        private readonly ISupplierService _supplierService;
        private readonly ICashInvoiceService _cashInvoiceService;
        private readonly IMapper _mapper;

        IReadOnlyList<SupplierDto> SuppliersDto = new List<SupplierDto>();
        ObservableCollection<ReturnCashInvoiceDto> observProductsLisLim = new ObservableCollection<ReturnCashInvoiceDto>();
        ObservableCollection<ReturnCashInvoiceDto> observProductsListFiltered = new ObservableCollection<ReturnCashInvoiceDto>();

        #endregion

        #region Constractor Region

        public Cashinvoice(ISupplierService supplierService, ICashInvoiceService cashInvoiceService)
        {
            InitializeComponent();
            _supplierService = supplierService;
            _cashInvoiceService = cashInvoiceService;
            Loaded += async (s, e) =>
            {
                cb_SuppliersName.ItemsSource = await _supplierService.GetAllAsync(); ;
                cb_SuppliersName.SelectedIndex = 0;
                await LoadInvoices();
                dgCashInvoice.ItemsSource =  observProductsLisLim;

            };

        }

        #endregion

        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            IReadOnlyList<ReturnCashInvoiceDto> invoiceDtos = await _cashInvoiceService.GetAllAsync();
            foreach (var product in invoiceDtos)
            {
                observProductsLisLim.Add(product);
                observProductsListFiltered.Add(product);
            }

        }


        #endregion
         
        #region Add Button Region

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? invoiceDate = txtInvoiceDate.SelectedDate;
            if (invoiceDate == null)
            {
                MessageBox.Show("من فضلك اختر تاريخ صحيح");
                return;
            } 

            SupplierDto selectedSupplier = (SupplierDto)cb_SuppliersName.SelectedItem;

            if (selectedSupplier == null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            AddCashInvoiceDto InputProduct = new AddCashInvoiceDto()
            {
                InvoiceDate = invoiceDate,
                SupplierId = selectedSupplier.Id
                
            };
             
            ReturnCashInvoiceDto CreateInvoiceDtoRespons = _cashInvoiceService.AddInvoice(InputProduct);
            if (CreateInvoiceDtoRespons is null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MessageBox.Show("تم إضافة الفاتوره الكاش بنجاح");

            cb_SuppliersName.SelectedIndex = 0;
            txtInvoiceDate.SelectedDate = null; 
            observProductsLisLim.Add(CreateInvoiceDtoRespons);
            observProductsListFiltered.Add(CreateInvoiceDtoRespons);

        }

        #endregion

        #region Delete Button Region

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool checkSoftDelete = false;
            if (dgCashInvoice.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }
            List<ReturnCashInvoiceDto> selectedItemsDto = dgCashInvoice.SelectedItems.Cast<ReturnCashInvoiceDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = await _cashInvoiceService.SoftDeleteAsync(item.Id);
                if (success)
                {
                    observProductsLisLim.Remove(item);
                    observProductsListFiltered.Remove(item);
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                string ValueOfString = "فاتوره كاش  ";
                if (deletedCount > 1)
                    ValueOfString = "من الفواتير الكاش  ";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي فاتوره بسبب خطأ ما");
            }

        }
        #endregion

        #region CashcInvoice Mouse Double Region

        private void dgCashInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgCashInvoice.SelectedItem is ReturnCashInvoiceDto selectedInvoice)
            {
                // افتح صفحة التفاصيل
                var detailsPage = new CashInvoiceDetailsPage(selectedInvoice);
                NavigationService?.Navigate(detailsPage);
            }
        }


        #endregion




        #region Back btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var invoicesRegion = new Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicesRegion(_companyNo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicesRegion);
        }

        #endregion
         
    }
}
