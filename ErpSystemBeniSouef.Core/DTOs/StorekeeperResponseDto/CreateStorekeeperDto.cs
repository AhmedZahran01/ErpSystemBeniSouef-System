using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.StorekeeperResponseDto
{
    public class CreateStorekeeperDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
