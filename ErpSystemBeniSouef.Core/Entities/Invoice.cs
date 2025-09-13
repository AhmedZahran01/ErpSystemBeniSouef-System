using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Enum;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Invoice :BaseEntity
    {
    public DateTime InvoiceDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? DueAmount { get; set; }
    public string? Notes { get; set; }
    public InvoiceType invoiceType { get; set; }
    public int? SupplierId { get; set; }
    public virtual Supplier? Supplier { get; set; }

    public ICollection<InvoiceItem>? Items { get; set; }
}
}
