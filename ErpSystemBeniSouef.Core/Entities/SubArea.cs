using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.Entities
{
    public class SubArea : BaseEntity
    { 
        public string Name { get; set; } = "";

        [ForeignKey("mainRegions")]
        public int MainAreaId { get; set; }
        public MainArea? mainRegions { get; set; }

         
    }
}
