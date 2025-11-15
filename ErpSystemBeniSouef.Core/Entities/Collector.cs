using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class Collector : BaseEntity
    {                   
        public string Name { get; set; }
        public ICollection<MonthlyInstallment> MonthlyInstallments { get; set; }
        public ICollection<CollectionBatch> CollectionBatches { get; set; }
    }
}
