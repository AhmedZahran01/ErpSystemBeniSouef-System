using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Commission : BaseEntity
    {
        public int RepresentativeId { get; set; }
        public Representative Representative { get; set; }

        public int InvoiceId { get; set; }
        public CustomerInvoice Invoice { get; set; }
        public int? InvoiceItemId { get; set; }
        public CustomerInvoiceItems InvoiceItem { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }

        public CommissionType Type { get; set; }
        public decimal TotalCommission { get; set; }
        public DateTime MonthDate { get; set; }
        public decimal CommissionAmount { get; set; }
        public string Note { get; set; }
        public decimal DeductedAmount { get; set; }
        public bool IsDeducted { get; set; }
        public DateTime? DeductedDate { get; set; }
    }

    public enum CommissionType
    {
        Earn = 1,
        Deduct = 2
    }
}
