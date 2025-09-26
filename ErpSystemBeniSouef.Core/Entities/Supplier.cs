using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Supplier: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<SupplierAccount> supplierAccounts { get; set; }
    }
}
