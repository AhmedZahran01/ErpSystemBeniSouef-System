namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class CustomerAccountDto
{
    public string CustomerName { get; set; } = string.Empty;
    public decimal Deposit { get; set; }
    public decimal TotalInvoices { get; set; }
    public decimal NetAmount { get; set; }
}