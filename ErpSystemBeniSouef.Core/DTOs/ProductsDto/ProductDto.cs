using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.DTOs.ProductsDto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal CommissionRate { get; set; }//معدل العمولة
        public decimal PurchasePrice { get; set; }//سعر الشراء
        public decimal SalePrice { get; set; }//سعر البيع
        public decimal ProfitMargin { get; set; }//هامش الربح
        public string CategoryName { get; set; } = string.Empty; 
        
        public int CategoryId { get; set; }
        public virtual CategoryDto? Category { get; set; } = new CategoryDto();

    }
}
