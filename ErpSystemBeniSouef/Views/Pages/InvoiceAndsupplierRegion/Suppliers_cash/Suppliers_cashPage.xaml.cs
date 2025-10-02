using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.SupplierCash;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Service.InvoiceServices.CashInvoiceService;
using ErpSystemBeniSouef.Service.SupplierService;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.Suppliers_cash
{
    /// <summary>
    /// Interaction logic for Suppliers_cashPage.xaml
    /// </summary>
    public partial class Suppliers_cashPage : Page
    { 
        #region Global Variables  Region
        private readonly int _companyNo = 1;
        int countDisplayNo = 0;
        private readonly ISupplierService _supplierService;
        private readonly ISupplierCashService _supplierCashService;
        private readonly IMapper _mapper;

        IReadOnlyList<SupplierRDto> SuppliersDto = new List<SupplierRDto>();
        ObservableCollection<ReturnSupplierCashDto> observProductsLisLim = new ObservableCollection<ReturnSupplierCashDto>();
        ObservableCollection<ReturnSupplierCashDto> observProductsListFiltered = new ObservableCollection<ReturnSupplierCashDto>();

        #endregion

        #region Constractor Region

        public Suppliers_cashPage(ISupplierService supplierService,
            ISupplierCashService cashInvoiceService)

        {
            InitializeComponent();
            _supplierService = supplierService;
            _supplierCashService = cashInvoiceService;
            Loaded += async (s, e) =>
            {
                SuppliersDto = await _supplierService.GetAllAsync();
                cb_SuppliersName.ItemsSource = SuppliersDto;
                cb_SuppliersName.SelectedIndex = 0;
                await LoadInvoices();
                DataGridOfSupplyCash.ItemsSource = observProductsLisLim;

            };
        }


        #endregion
         
        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            IReadOnlyList<ReturnSupplierCashDto> invoiceDtos = await _supplierCashService.GetAllSupplierAccounts();
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

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? invoiceDate = txtInvoiceDate.SelectedDate;
            string Amounttxt = AmounttxtValue.Text;
            string notesTxt = TxtNotes.Text;
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

            if ( !decimal.TryParse(Amounttxt, out decimal decimalAmount)   )
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            AddSupplierCashDto InputProduct = new AddSupplierCashDto()
            {
                PaymentDate = invoiceDate,
                SupplierId = selectedSupplier.Id,
                Amount = decimalAmount,
                Notes = notesTxt,

            };

            ReturnSupplierCashDto CreateInvoiceDtoRespons = await _supplierCashService.AddSupplierCash(InputProduct);
            if (CreateInvoiceDtoRespons is null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MessageBox.Show("تم إضافة الفاتوره الكاش بنجاح");

            //cb_SuppliersName.SelectedIndex = 0;
            txtInvoiceDate.SelectedDate = null;
            observProductsLisLim.Add(CreateInvoiceDtoRespons);
            observProductsListFiltered.Add(CreateInvoiceDtoRespons);

        }

        #endregion
         
        #region Delete Button Region

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool checkSoftDelete = false;
            if (DataGridOfSupplyCash.SelectedItems.Count == 0)
            {
                MessageBox.Show("من فضلك اختر علي الاقل صف قبل الحذف");
                return;
            }
            List<ReturnSupplierCashDto> selectedItemsDto = DataGridOfSupplyCash.SelectedItems.Cast<ReturnSupplierCashDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = await _supplierCashService.SoftDeleteAsync(item.Id);
                if (success)
                {
                    observProductsLisLim.Remove(item);
                    observProductsListFiltered.Remove(item);
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                string ValueOfString = "نقديه مورد  ";
                if (deletedCount > 1)
                    ValueOfString = "نقديه موردين   ";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي فاتوره بسبب خطأ ما");
            }

        }
        #endregion
         
        #region dgMainRegions_SelectionChanged Region

        private void dgAllInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridOfSupplyCash.SelectedItem is ReturnSupplierCashDto selected)
            {
                SupplierRDto selectedSupplier = SuppliersDto.FirstOrDefault(s => s.Id == selected.SupplierId);
                cb_SuppliersName.SelectedItem = selectedSupplier;
                txtInvoiceDate.SelectedDate = selected.PaymentDate;
                AmounttxtValue.Text = selected.Amount.ToString();
                TxtNotes.Text = selected.Notes;
                editBtn.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Btn Edit Click  Region

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridOfSupplyCash.SelectedItem is not ReturnSupplierCashDto selected)
            {
                MessageBox.Show("من فضلك اختر فاتوره محدده للتعديل");
                return;
            }

            DateTime UpdateInvoiceDate = txtInvoiceDate.SelectedDate ?? DateTime.UtcNow;

            int updateSupplierId = ((SupplierRDto)cb_SuppliersName.SelectedItem).Id;

            string notesTxt = TxtNotes.Text;

            string Amounttxt = AmounttxtValue.Text;
            if (!decimal.TryParse(Amounttxt, out decimal decimalAmount))
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            var updateDto = new UpdateSupplierCashDto()
            {
                Id = selected.Id,
                TransactionDate = UpdateInvoiceDate,
                SupplierId = updateSupplierId,
                Notes = notesTxt,
                Amount = decimalAmount,
                
            };

            bool success = _supplierCashService.Update(updateDto);

            if (success)
            {
                SupplierRDto supplierDto = SuppliersDto.FirstOrDefault(i => i.Id == selected.SupplierId);

                selected.SupplierId = ((SupplierRDto)cb_SuppliersName.SelectedItem).Id; 
                selected.SupplierName = ((SupplierRDto)cb_SuppliersName.SelectedItem).Name;
                selected.PaymentDate = UpdateInvoiceDate;
                selected.Amount = updateDto.Amount;
                selected.Notes = updateDto.Notes;
                txtInvoiceDate.SelectedDate = UpdateInvoiceDate;
                MessageBox.Show("تم تعديل المنطقة بنجاح");
                DataGridOfSupplyCash.Items.Refresh(); // لتحديث الجدول
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }
        }



        #endregion




         
        #region Back btn Region

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var invoicesRegion = new Views.Pages.InvoiceAndsupplierRegion.InvoiceAndsupplierRegion(_companyNo);
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(invoicesRegion);
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
                DataGridOfSupplyCash.ItemsSource = filtered;
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
