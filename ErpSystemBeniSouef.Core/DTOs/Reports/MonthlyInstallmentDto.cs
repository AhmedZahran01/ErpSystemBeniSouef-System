namespace ErpSystemBeniSouef.Core.DTOs.Reports;

public class MonthlyInstallmentDto
{
    public DateTime MonthDate { get; set; }
    public decimal Amount { get; set; }
    public decimal CollectedAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsDelayed { get; set; }
}