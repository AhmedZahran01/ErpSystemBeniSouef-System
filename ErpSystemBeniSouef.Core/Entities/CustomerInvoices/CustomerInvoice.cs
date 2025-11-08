using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices
{
    public class CustomerInvoice:BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Navigation
        public ICollection<CustomerInvoiceItems> Items { get; set; }
        public ICollection<InstallmentPlan> Installments { get; set; }
    }
}
