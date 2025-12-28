namespace ErpSystemBeniSouef.Core.DTOs.CollectionDto;

public class InstallmentCollectionDto
{
    public int InstallmentId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal CollectedAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsDelayed { get; set; }
    public decimal RemainingAmount => Amount - CollectedAmount;
}