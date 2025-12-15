namespace ErpSystemBeniSouef.Core.DTOs.Receipt;

public class GetAllReceiptsDto
{
    public int MonthlyInstallmentId { get; set; }
    public int CustomerNumber { get; set; }
    public string CustomerName { get; set; } = string.Empty;    
    public string MobileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string NationlNumber { get; set; } = string.Empty;
    public decimal Deposite { get; set; }
    public string CollectorName { get; set; } = string.Empty;
    public string RepresentativeName { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public DateTime MonthDate { get; set; }
    public decimal TotalPrice { get; set; }

    public List<InstallmentPlanDto> Plans { get; set; } = [];
    public List<CustomerInvoiceItemDto> Items { get; set; } = [];
}