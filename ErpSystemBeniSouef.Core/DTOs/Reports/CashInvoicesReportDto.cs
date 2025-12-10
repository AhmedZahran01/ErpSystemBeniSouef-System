namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class CashInvoicesReportDto
{
    public string ProductType { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quentity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quentity * UnitPrice;
}