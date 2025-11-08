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

        public DateTime MonthDate { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal DeductedAmount { get; set; }
        public string Note { get; set; }

        public bool IsDeducted { get; set; }
        public DateTime? DeductedDate { get; set; }
    }
}
