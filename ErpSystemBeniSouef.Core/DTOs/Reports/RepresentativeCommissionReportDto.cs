namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class RepresentativeCommissionReportDto
{
    public string ProductName { get; set; } = string.Empty;
    public decimal CommissionAmount { get; set; }
    public int QuantitySold { get; set; }
    public decimal TotalPercentage { get; set; }
}