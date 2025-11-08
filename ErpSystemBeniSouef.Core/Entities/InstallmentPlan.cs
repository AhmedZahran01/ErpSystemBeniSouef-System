using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class InstallmentPlan:BaseEntity
    {
        public int InvoiceId { get; set; }
        public CustomerInvoice Invoice { get; set; }

        //public int SystemNumber { get; set; } 
        public int NumberOfMonths { get; set; }
        public decimal Amount { get; set; }
    }
}
