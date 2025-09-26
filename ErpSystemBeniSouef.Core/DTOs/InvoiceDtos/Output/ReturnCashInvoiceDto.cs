using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output
{
    public class ReturnCashInvoiceDto
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual SupplierRDto Supplier { get; set; }

        public string SupplierName { get; set; }
        public int SupplierId { get; set; }

    }
    public class InvoiceDetailsDto
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string SupplierName { get; set; }
        public string? Notes { get; set; }
    }

    public class InvoiceItemDetailsDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
        public string? Notes { get; set; }
    }
}
