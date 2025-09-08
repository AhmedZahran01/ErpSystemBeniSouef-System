using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class SubArea : BaseEntity
    { 
        public string Name { get; set; } = "";

        public int MainAreaId { get; set; }
        public MainArea? MainArea { get; set; }
    }
}
