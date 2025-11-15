using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices
{
    public class CollectionBatch : BaseEntity
    {
        public int CollectorId { get; set; }
        public Collector Collector { get; set; }
      
        public DateTime MonthDate { get; set; }
        public decimal TotalAssignedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CarriedOverAmount { get; set; } // مؤجلات من شهر سابق

        public ICollection<CollectionEntry> Entries { get; set; } = new List<CollectionEntry>();

        public ICollection<MonthlyInstallment> Installments { get; set; }
    }
}
