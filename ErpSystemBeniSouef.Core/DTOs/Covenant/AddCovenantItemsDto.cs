using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.Covenant
{
    public class AddCovenantItemsDto
    {
        public int CovenantId { get; set; }
        public List<CovenantItemsDto> CovenantItems { get; set; }
    }
    public class CovenantItemsDto {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }

    }
}
