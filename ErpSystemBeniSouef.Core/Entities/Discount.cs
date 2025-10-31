using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Discount : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int InvoiceId { get; set; }
        public CustomerInvoice Invoice { get; set; }

        public int CollectorId { get; set; }
        public Collector Collector { get; set; }

        public DateTime MonthDate { get; set; }       // The month this discount applies to
        public decimal Amount { get; set; }           // Discount amount
        public string Reason { get; set; }            // e.g., “Payment delay” or “Special offer”
        public bool IsReversed { get; set; }          // If reversed due to customer deletion
        public DateTime? ReversedDate { get; set; }
    }
}
