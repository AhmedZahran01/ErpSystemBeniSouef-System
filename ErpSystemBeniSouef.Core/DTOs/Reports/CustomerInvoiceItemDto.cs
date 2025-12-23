using ErpSystemBeniSouef.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class CustomerInvoiceItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    
    //[ForeignKey(nameof(Product))]
    //public int productId { get; set; }
    //public Product? product { get; set; }
}