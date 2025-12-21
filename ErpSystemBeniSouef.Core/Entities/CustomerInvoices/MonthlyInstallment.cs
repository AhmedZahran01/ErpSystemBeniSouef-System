using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices
{
    public class MonthlyInstallment : BaseEntity
    {
        public int InvoiceId { get; set; }
        public CustomerInvoice Invoice { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int CollectorId { get; set; } // المحصل
        public Collector Collector { get; set; }

        public DateTime MonthDate { get; set; } // لكل شهر
        public decimal Amount { get; set; }     // قيمة القسط
        public decimal CollectedAmount { get; set; } = 0;
        // public bool IsCollected => CollectedAmount >= Amount;
        public bool IsPaid { get; set; } = false;
        public bool IsDelayed { get; set; } // هل اتأجل للشهر اللي بعده؟
    }
}
