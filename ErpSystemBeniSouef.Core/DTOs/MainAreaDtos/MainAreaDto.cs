using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Dtos.MainAreaDto
{
    public class MainAreaDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StartNumbering { get; set; } 
        public bool IsDeleted { get; set; } = false;

    }
}
