using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos
{
    public class SupplierAccountReportDto
    {
        public string SupplierName { get; set; }
        public List<SupplierInvoiceDto> Invoices { get; set; }
        public List<SupplierCashDto> Payments { get; set; }
    }

     
}
