namespace ErpSystemBeniSouef.Core.DTOs.CollectionDto;

public class CollectorInstallmentsResultDto
{
    public string CollectorName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<InstallmentCollectionDto> Installments { get; set; } = [];
}