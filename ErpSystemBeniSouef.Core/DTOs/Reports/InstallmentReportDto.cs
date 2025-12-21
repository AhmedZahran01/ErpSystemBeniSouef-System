namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class InstallmentReportDto
{
    public DateTime InvoiceDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Deposit { get; set; }

    public string Plans { get; set; } = string.Empty;
    public string Items { get; set; } = string.Empty;
}