using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;

namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class InstallmentReportDto
{
    public DateTime InvoiceDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Deposit { get; set; }

    public List<InstallmentPlanDto> Plans { get; set; } = [];
    public List<CustomerInvoiceItemDto> Items { get; set; } = [];
}