using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.Contract.Invoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.CashInvoice;
using ErpSystemBeniSouef.Core.Contract.Invoice.DueInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DueInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DueInvoiceDtos;
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
        private readonly IDueInvoiceService _dueInvoiceService;
        private readonly IMapper _mapper;

        IReadOnlyList<SupplierRDto> SuppliersDto = new List<SupplierRDto>();
        ObservableCollection<DueInvoiceDetailsDto> observDueInvoiceLisLim = new ObservableCollection<DueInvoiceDetailsDto>();
        ObservableCollection<DueInvoiceDetailsDto> observDueInvoiceFiltered = new ObservableCollection<DueInvoiceDetailsDto>();

        #endregion

        #region Constractor Region

        public DueInvoicePage(ISupplierService supplierService, IDueInvoiceService dueInvoiceService)
        {
            InitializeComponent();
            _supplierService = supplierService;
            _dueInvoiceService = dueInvoiceService;
            Loaded += async (s, e) =>
            {
                SuppliersDto = await _supplierService.GetAllAsync();
                cb_SuppliersName.ItemsSource = SuppliersDto;
                cb_SuppliersName.SelectedIndex = 0;
                await LoadInvoices();
                dgCashInvoice.ItemsSource = observDueInvoiceFiltered;

            };

        }

        #endregion

        #region LoadInvoices Dta Region

        private async Task LoadInvoices()
        {
            IReadOnlyList<DueInvoiceDetailsDto> invoiceDtos = await _dueInvoiceService.GetAllAsync();
            observDueInvoiceLisLim.Clear();
            observDueInvoiceFiltered.Clear();
            foreach (var product in invoiceDtos)
            {
                product.DisplayId = countDisplayNo + 1;
                observDueInvoiceLisLim.Add(product);
                observDueInvoiceFiltered.Add(product);
                countDisplayNo++;

            }

        }


        #endregion

        #region Add Button Region

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
             

            DateTime invoiceDate = txtInvoiceDate.SelectedDate ?? DateTime.UtcNow;
            if (txtInvoiceDate.SelectedDate == null)
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

            if (!decimal.TryParse(DueTxtAmout.Text, out decimal DueAmout)
                      || !(DueAmout > 0))
            {
                MessageBox.Show(" من فضلك ادخل نسبه تقسيط رقم اكبر من  0   ");
                return;
            }

            AddDueInvoiceDto InputProduct = new AddDueInvoiceDto()
            {
                InvoiceDate = invoiceDate,
                SupplierId = selectedSupplier.Id,
                DueAmount = DueAmout
            };

            DueInvoiceDetailsDto CreateInvoiceDtoRespons = await _dueInvoiceService.AddDueInvoice(InputProduct);
            if (CreateInvoiceDtoRespons is null)
            {
                MessageBox.Show("من فضلك ادخل بيانات صحيحة");
                return;
            }

            MessageBox.Show("تم إضافة الفاتوره الاجل بنجاح");

            cb_SuppliersName.SelectedIndex = 0;
            txtInvoiceDate.SelectedDate = null;
            CreateInvoiceDtoRespons.DisplayId = countDisplayNo + 1;
            CreateInvoiceDtoRespons.SupplierId = selectedSupplier.Id;
            observDueInvoiceLisLim.Add(CreateInvoiceDtoRespons);
            observDueInvoiceFiltered.Add(CreateInvoiceDtoRespons);
            countDisplayNo+=1; 

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

            List<DueInvoiceDetailsDto> selectedItemsDto = dgCashInvoice.SelectedItems.Cast<DueInvoiceDetailsDto>().ToList();
            int deletedCount = 0;
            foreach (var item in selectedItemsDto)
            {
                bool success = await _dueInvoiceService.SoftDeleteAsync(item.Id);
                if (success)
                {
                    observDueInvoiceLisLim.Remove(item);
                    observDueInvoiceFiltered.Remove(item);
                    dgCashInvoice.Items.Refresh();
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                string ValueOfString = "فاتوره اجل  ";
                if (deletedCount > 1)
                    ValueOfString = "من الفواتير الاجل   ";
                MessageBox.Show($"تم حذف {deletedCount} {ValueOfString} ");
            }
            else
            {
                MessageBox.Show("لم يتم حذف أي فاتوره بسبب خطأ ما");
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
            if (dgCashInvoice.SelectedItem is DueInvoiceDetailsDto selected)
            {
                SupplierRDto selectedSupplier = SuppliersDto.FirstOrDefault(s => s.Id == selected.SupplierId);
                cb_SuppliersName.SelectedItem = selectedSupplier;
                txtInvoiceDate.SelectedDate = selected.InvoiceDate;
                DueTxtAmout.Text = selected.DueAmount.ToString();
                editBtn.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Btn Edit Click  Region


        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCashInvoice.SelectedItem is not DueInvoiceDetailsDto selected)
            {
                MessageBox.Show("من فضلك اختر فاتوره محدده للتعديل");
                return;
            }

            DateTime updateInvoiceDate = txtInvoiceDate.SelectedDate ?? DateTime.UtcNow;

            if (!decimal.TryParse(DueTxtAmout.Text, out decimal dueAmount) || !(dueAmount > 0))
            {
                MessageBox.Show(" من فضلك ادخل نسبه تقسيط رقم اكبر من 0   ");
                return;
            }

            SupplierRDto supplier = cb_SuppliersName.SelectedItem as SupplierRDto;
            if (supplier == null)
            {
                MessageBox.Show("من فضلك اختر مورد صحيح");
                return;
            }

            var updateDto = new UpdateDueInvoiceDto()
            {
                Id = selected.Id,
                InvoiceDate = updateInvoiceDate,
                SupplierId = supplier.Id,
                DueAmount = dueAmount
            };

            bool success = _dueInvoiceService.Update(updateDto);

            if (success)
            {
                // التغييرات هتتحدث أوتوماتيك بسبب INotifyPropertyChanged
                selected.InvoiceDate = updateInvoiceDate;
                selected.SupplierId = supplier.Id;
                selected.SupplierName = supplier.Name;
                selected.DueAmount = dueAmount;

                MessageBox.Show("تم تعديل الفاتوره الاجل بنجاح");
            }
            else
            {
                MessageBox.Show("حدث خطأ أثناء التعديل");
            }
        }


        //private void BtnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgCashInvoice.SelectedItem is not DueInvoiceDetailsDto selected)
        //    {
        //        MessageBox.Show("من فضلك اختر فاتوره محدده للتعديل");
        //        return;
        //    }
        //    DateTime UpdateInvoiceDate = txtInvoiceDate.SelectedDate ?? DateTime.UtcNow;

        //    if (!decimal.TryParse(DueTxtAmout.Text, out decimal DueAmout)
        //            || !(DueAmout > 0))
        //    {
        //        MessageBox.Show(" من فضلك ادخل نسبه تقسيط رقم اكبر من  0   ");
        //        return;
        //    }
        //    decimal dueAmount = DueAmout;

        //    int updateSupplierId = ((SupplierRDto)cb_SuppliersName.SelectedItem).Id;

        //    var updateDto = new UpdateDueInvoiceDto()
        //    {
        //        Id = selected.Id,
        //        InvoiceDate = UpdateInvoiceDate,
        //        SupplierId = updateSupplierId,
        //        DueAmount = dueAmount
        //    };

        //    bool success = _dueInvoiceService.Update(updateDto);

        //    if (success)
        //    {
        //        SupplierRDto supplierDto = SuppliersDto.FirstOrDefault(i => i.Id == selected.SupplierId);

        //        selected.SupplierId = ((SupplierRDto)cb_SuppliersName.SelectedItem).Id;
        //        selected.SupplierName = ((SupplierRDto)cb_SuppliersName.SelectedItem).Name;
        //        selected.InvoiceDate = UpdateInvoiceDate;
        //        txtInvoiceDate.SelectedDate = UpdateInvoiceDate;
        //        MessageBox.Show("تم تعديل الفاتوره الاجل بنجاح");
        //        dgCashInvoice.Items.Refresh(); // لتحديث الجدول
        //    }
        //    else
        //    {
        //        MessageBox.Show("حدث خطأ أثناء التعديل");
        //    }
        //}

        #endregion

        #region Search By Item FullName  Region

        private void SearchByItemFullNameBox_TextChanged(object sender, TextChangedEventArgs e)
        { 
            var query = SearchByItemTextBox.Text?.ToLower() ?? "";

            // فلترة النتائج
            var filtered = observDueInvoiceLisLim
                .Where(i => i.SupplierName != null && i.SupplierName.ToLower().Contains(query))
                .ToList();
            // تحديث الـ DataGrid
            observDueInvoiceFiltered.Clear();
            foreach (var item in filtered)
            {
                observDueInvoiceFiltered.Add(item);
            }

            // تحديث الاقتراحات
            var suggestions = filtered.Select(i => i.SupplierName);
            if (suggestions.Any())
            {
                dgCashInvoice.ItemsSource = filtered; 
            } 

        }

        #endregion






        #region CashcInvoice Mouse Double Region

        private void dgCashInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgCashInvoice.SelectedItem is DueInvoiceDetailsDto selectedInvoice)
            {
                var productService = App.AppHost.Services.GetRequiredService<IProductService>();
                var dueInvoiceItemService = App.AppHost.Services.GetRequiredService<IDueInvoiceItemService>();
                var mapper = App.AppHost.Services.GetRequiredService<IMapper>();

                // افتح صفحة التفاصيل
                var detailsPage = new DueInvoiceDetailsPage(selectedInvoice, productService, dueInvoiceItemService, mapper);
                NavigationService?.Navigate(detailsPage);
            }
        }


        #endregion


    }
}
