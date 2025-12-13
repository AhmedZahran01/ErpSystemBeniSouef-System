namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class PrintCashInvoicesDto
{
    public string Representative { get; set; } = string.Empty;
    public string SubArea { get; set; } = string.Empty;
    public string MainArea { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
}