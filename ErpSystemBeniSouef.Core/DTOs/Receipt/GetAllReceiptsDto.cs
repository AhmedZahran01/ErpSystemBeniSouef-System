namespace ErpSystemBeniSouef.Core.DTOs.Receipt;

public class GetAllReceiptsDto
{
    public int DisplayUIId { get; set; }
    public int MonthlyInstallmentId { get; set; }
    public int CustomerNumber { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string Address { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public string RepresentativeName { get; set; } = string.Empty;
    public string NationlNumber { get; set; } = string.Empty;
    public decimal Deposit { get; set; }
    public string Plans { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Items { get; set; } = string.Empty;
    public string CollectorName { get; set; } = string.Empty;
    public DateTime FirstInvoiceDate { get; set; }
    public bool IsPaid { get; set; }
    public decimal InstallmentAmount { get; set; }
    public DateTime InstallmentDueDate { get; set; }
    public string InstallmentAmountText { get; set; } = string.Empty;
}