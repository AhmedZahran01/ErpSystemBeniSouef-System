using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.output
{
    public class ReturnCustomerInvoiceListDTO
    {
        public int serialNumber { get; set; }
        public int CustomerNumber { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }

        public string NationalNumber { get; set; }
        public decimal Deposit { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime FirstInvoiceDate { get; set; }
        public string  SubAreaName { get; set; }
        public string CollectorName { get; set; }
        public string RepresentativeName { get; set; }

    }
}
