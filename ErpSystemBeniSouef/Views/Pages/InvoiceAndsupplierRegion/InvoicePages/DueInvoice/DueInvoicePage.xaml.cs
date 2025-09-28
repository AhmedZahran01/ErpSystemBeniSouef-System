using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages;
using Microsoft.Extensions.DependencyInjection;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.DueInvoice
{
    /// <summary>
    /// Interaction logic for DueInvoicePage.xaml
    /// </summary>
    public partial class DueInvoicePage : Page
    {  
        #region Global Variables  Region
        private readonly int _companyNo = 1;
        int countDisplayNo = 0;
        private readonly ISupplierService _supplierService;
        private readonly ICashInvoiceService _cashInvoiceService;
        private readonly IMapper _mapper;

        IReadOnlyList<SupplierRDto> SuppliersDto = new List<SupplierRDto>();
        ObservableCollection<CashInvoiceDto> observProductsLisLim = new ObservableCollection<CashInvoiceDto>();
        ObservableCollection<CashInvoiceDto> observProductsListFiltered = new ObservableCollection<CashInvoiceDto>();

        #endregion

        #region Constractor Region

        public DueInvoicePage(ISupplierService supplierService, ICashInvoiceService cashInvoiceService)
        {
            InitializeComponent();
            _supplierService = supplierService;
            _cashInvoiceService = cashInvoiceService;
            Loaded += async (s, e) =>
            {
                SuppliersDto = await _supplierService.GetAllAsync();
                cb_SuppliersName.ItemsSource = SuppliersDto;
                cb_SuppliersName.SelectedIndex = 0;
                await LoadInvoices();
                dgCashInvoice.ItemsSource = observProductsLisLim;

            };

        }

        #endregion

        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            IReadOnlyList<CashInvoiceDto> invoiceDtos = await _cashInvoiceService.GetAllAsync();
            observProductsLisLim.Clear();
            observProductsListFiltered.Clear();
            foreach (var product in invoiceDtos)
            {
                product.DisplayId = countDisplayNo + 1;
                observProductsLisLim.Add(product);
                observProductsListFiltered.Add(product);
                countDisplayNo++;

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

            SupplierRDto selectedSupplier = (SupplierRDto)cb_SuppliersName.SelectedItem;

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

            CashInvoiceDto CreateInvoiceDtoRespons = _cashInvoiceService.AddInvoice(InputProduct);
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
            List<CashInvoiceDto> selectedItemsDto = dgCashInvoice.SelectedItems.Cast<CashInvoiceDto>().ToList();
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
            if (dgCashInvoice.SelectedItem is CashInvoiceDto selectedInvoice)
            {
                var productService = App.AppHost.Services.GetRequiredService<IProductService>();
                var cashInvoiceItemsService = App.AppHost.Services.GetRequiredService<ICashInvoiceItemsService>();
                var mapper = App.AppHost.Services.GetRequiredService<IMapper>();

                // افتح صفحة التفاصيل
                var detailsPage = new CashInvoiceDetailsPage(selectedInvoice, productService, cashInvoiceItemsService, mapper);
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

        #region dgMainRegions_SelectionChanged Region

        private void dgAllInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCashInvoice.SelectedItem is CashInvoiceDto selected)
            {
                SupplierRDto selectedSupplier = SuppliersDto.FirstOrDefault(s => s.Id == selected.SupplierId);
                cb_SuppliersName.SelectedItem = selectedSupplier;
                txtInvoiceDate.SelectedDate = selected.InvoiceDate;
                editBtn.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Btn Edit Click  Region

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCashInvoice.SelectedItem is not CashInvoiceDto selected)
            {
                MessageBox.Show("من فضلك اختر فاتوره محدده للتعديل");
                return;
            }

            DateTime UpdateInvoiceDate = txtInvoiceDate.SelectedDate ?? DateTime.UtcNow;
            //if(UpdateInvoiceDate is null )
            //{
            //    UpdateInvoiceDate = DateTime.UtcNow;
            //}

            int updateSupplierId = ((SupplierRDto)cb_SuppliersName.SelectedItem).Id;

            var updateDto = new UpdateInvoiceDto()
            {
                Id = selected.Id,
                InvoiceDate = UpdateInvoiceDate,
                SupplierId = updateSupplierId
            };

            bool success = _cashInvoiceService.Update(updateDto);

            if (success)
            {
                SupplierRDto supplierDto = SuppliersDto.FirstOrDefault(i => i.Id == selected.SupplierId);

                selected.SupplierId = ((SupplierRDto)cb_SuppliersName.SelectedItem).Id;
                selected.Supplier = ((SupplierRDto)cb_SuppliersName.SelectedItem);
                selected.SupplierName = ((SupplierRDto)cb_SuppliersName.SelectedItem).Name;
                selected.InvoiceDate = UpdateInvoiceDate;
                txtInvoiceDate.SelectedDate = UpdateInvoiceDate;
                MessageBox.Show("تم تعديل المنطقة بنجاح");
                dgCashInvoice.Items.Refresh(); // لتحديث الجدول
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }
        }



        #endregion
         
        #region Search By Item FullName  Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observProductsLisLim
                .Where(i => i.SupplierName != null && i.SupplierName.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observProductsListFiltered.Clear();
            foreach (var item in filtered)
            {
                observProductsListFiltered.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.SupplierName);
            if (suggestions.Any())
            {
                dgCashInvoice.ItemsSource = filtered;
                //SuggestionsItemsListBox.ItemsSource = suggestions;
                //SuggestionsPopup.IsOpen = true;
            }
            else
            {
                //SuggestionsPopup.IsOpen = false;
            }

        }

        #endregion
         

    }
}
