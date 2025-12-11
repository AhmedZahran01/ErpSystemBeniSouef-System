namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class CovenantReportRowDto
{
    public int CustomerNumber { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal ProductRate { get; set; }
    public decimal TotalRate { get; set; }
    public decimal CommisionRate { get; set; }
    public decimal TotalCommisionRate { get; set; }
}