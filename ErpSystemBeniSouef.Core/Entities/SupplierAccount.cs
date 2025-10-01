using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class SupplierAccount:BaseEntity
    { 
        public DateTime? TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
