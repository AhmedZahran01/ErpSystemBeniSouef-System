using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.Entities;
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

namespace ErpSystemBeniSouef.Views.Pages.InvoiceAndsupplierRegion.InvoicePages.InvoicePages
{
    /// <summary>
    /// Interaction logic for CashInvoiceDetailsPage.xaml
    /// </summary>
    public partial class CashInvoiceDetailsPage : Page
    {
        private readonly ReturnCashInvoiceDto _invoice;

        public CashInvoiceDetailsPage(ReturnCashInvoiceDto invoice)
        {
            InitializeComponent();
            _invoice = invoice;
            DataContext = _invoice;
            InvoiceIdTxt.Text = invoice.Id.ToString();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
