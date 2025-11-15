using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices
{
    public class CollectionEntry : BaseEntity
    {
        public int MonthlyInstallmentId { get; set; }
        public MonthlyInstallment Installment { get; set; }
        public int? CollectionBatchId { get; set; }
        public CollectionBatch CollectionBatch { get; set; }
        public DateTime CollectionDate { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
