using ErpSystemBeniSouef.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.SupplierCashDtos
{

    public class SupplierInvoiceDto
    {
        public int Id { get; set; }
        public int DisplayId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string? Notes { get; set; }

        public InvoiceType invoiceType { get; set; }

        public string MoveType { get; set; } = "";
        public string SaleType { get; set; } = "";
    }
}
