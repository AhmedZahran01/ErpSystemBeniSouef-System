using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.SubAreaDtos
{
    public class RepresentativeDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int UserNumber { get; set; }    
        public string Password { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
