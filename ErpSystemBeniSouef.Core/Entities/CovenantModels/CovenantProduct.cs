using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities.CovenantModels
{
    public class CovenantProduct:BaseEntity
    {
        public int CovenantId { get; set; }
        public Covenant Covenant { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
    }
}
