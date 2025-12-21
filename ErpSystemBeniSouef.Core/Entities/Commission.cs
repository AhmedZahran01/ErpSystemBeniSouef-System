using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;
using ErpSystemBeniSouef.Core.Enum;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Commission : BaseEntity
    {
        public int RepresentativeId { get; set; }
        public Representative Representative { get; set; } = default!;

        public int InvoiceId { get; set; }
        public CustomerInvoice Invoice { get; set; } = default!;

        public int InvoiceItemId { get; set; }
        public CustomerInvoiceItems InvoiceItem { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public CommissionType Type { get; set; }
        public decimal TotalCommission { get; set; }
        public DateTime MonthDate { get; set; }
        public decimal CommissionAmount { get; set; }
        public string Note { get; set; } = string.Empty;
        public decimal DeductedAmount { get; set; }
        public bool IsDeducted { get; set; }
        public DateTime? DeductedDate { get; set; }
    }
}