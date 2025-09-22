using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.RepresentativeDto
{
    public class CreateRepresentativeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserNumber { get; set; }     

        public string Password { get; set; }
    }
}
