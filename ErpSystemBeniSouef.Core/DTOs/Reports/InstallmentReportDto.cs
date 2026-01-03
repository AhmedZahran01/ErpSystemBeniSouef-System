namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class InstallmentReportDto
{
    public int DisplayUiId { get; set; } = 0;
    public string CustomerName { get; set; } = string.Empty;
    public int CustomerNumber { get; set; } = 0;
    public DateTime InvoiceDate { get; set; }
    public decimal Deposit { get; set; }

    public string Plans { get; set; } = string.Empty;
    public string Items { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }
}