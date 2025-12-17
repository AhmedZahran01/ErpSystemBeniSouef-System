namespace ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.PayAnIncrease;

public class UpdateMonthlyInstallmentsDto
{
    public int InvoiceId { get; set; }
    public List<MonthlyInstallmentDto> Installments { get; set; } = [];
}