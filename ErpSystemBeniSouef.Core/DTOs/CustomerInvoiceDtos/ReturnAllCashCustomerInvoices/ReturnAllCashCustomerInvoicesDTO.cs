using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.ReturnAllCashCustomerInvoices
{
    public class ReturnAllCashCustomerInvoicesDTO
    {
        public int serialNumber { get; set; }
      
        public DateTime SaleDate { get; set; }
        public string MainAreaName { get; set; }
        public string SubAreaName { get; set; }
        public string CollectorName { get; set; }
        public string RepresentativeName { get; set; }
        public decimal total { get; set; }
    }
}
