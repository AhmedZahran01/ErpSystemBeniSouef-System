using ErpSystemBeniSouef.Core.Contract.PettyCash;
using ErpSystemBeniSouef.Core.DTOs.PettyCash;
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

namespace ErpSystemBeniSouef.Views.Pages.SundriesRegion
{
    /// <summary>
    /// Interaction logic for SundriesPage.xaml
    /// </summary>
    public partial class SundriesPage : Page
    {
        private readonly IPettyCashService _pettyCashService;

        public SundriesPage(IPettyCashService pettyCashService)
        {
            InitializeComponent();
            _pettyCashService = pettyCashService;
        }

        private async void AddSundriesBtn_Click(object sender, RoutedEventArgs e)
        {
            string sundriesReason = SundriesReason.Text;
            string sundriesTotal = SundriesTotal.Text;

            DateTime? invoiceDate = SundriesDate.SelectedDate;
            if (invoiceDate == null)
            {
                MessageBox.Show("من فضلك اختر تاريخ صحيح");
                return;
            }

            if (!decimal.TryParse(sundriesTotal, out decimal amount))
            {
                MessageBox.Show("من فضلك ادخل مبلغ صحيح");
                return;
            }

            AddPettyCashDto addPettyCashDto = new AddPettyCashDto
            {
                Amount = amount,
                Reason = sundriesReason,
                Date = invoiceDate.Value
            };

            bool added = await _pettyCashService.AddPettyCash(addPettyCashDto);

            if (added)
                MessageBox.Show("تم الإضافة بنجاح");
            else
                MessageBox.Show("حدث خطأ أثناء الإضافة");
        }


        //private async void AddSundriesBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    string sundriesReason = SundriesReason.Text;
        //    string sundriesTotal = SundriesTotal.Text;

        //    DateTime? invoiceDate = SundriesDate.SelectedDate;
        //    if (invoiceDate == null)
        //    {
        //        MessageBox.Show("من فضلك اختر تاريخ صحيح");
        //        return;
        //    }
        //    if (!decimal.TryParse(sundriesTotal, out decimal CommissionRate))
        //    {
        //        MessageBox.Show("من فضلك ادخل بيانات صحيحة");
        //        return;
        //    }
        //    AddPettyCashDto addPettyCashDto = new AddPettyCashDto()
        //    {
        //        Amount = CommissionRate,
        //        Reason = sundriesReason,
        //        Date = invoiceDate ?? DateTime.UtcNow,
        //    };
        //    var AddSundries = await _pettyCashService.AddPettyCash(addPettyCashDto);
        //    if (AddSundries)
        //    {
        //        MessageBox.Show("تم الاضافه بنجاح");
        //        return;
        //    }
        //    else
        //    { 
        //        MessageBox.Show("من فضلك ادخل بيانات صحيحة");
        //        return;
        //    }
        //}

    }
}
