using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output
{
    public class SupplierAccountReportDto
    {
        public string SupplierName { get; set; }
        public List<SupplierInvoiceDto> Invoices { get; set; }
        public List<SupplierCashDto> Payments { get; set; }
    }

    public class SupplierInvoiceDto
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string? Notes { get; set; }
    }

    public class SupplierCashDto
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
