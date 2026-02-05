using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class RepresentativeWithdrawal : BaseEntity
    {
        public DateTime Date { get; set; }
        public string? Reason { get; set; }
        public decimal Amount { get; set; }
        public int representativeId { get; set; }

        public Representative representative { get; set; }
    }
}
