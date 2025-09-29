using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.Enum;

namespace ErpSystemBeniSouef.Core.Entities.CovenantModels
{
    public class Covenant:BaseEntity
    {
        public DateTime CovenantDate { get; set; }
        public DateTime MonthDate { get; set; }
        public int RepresentativeId { get; set; }
        public Representative Representative { get; set; }
        public string CovenantType { get; set; }

        public ICollection<CovenantProduct> CovenantProducts { get; set; }
    }
}
