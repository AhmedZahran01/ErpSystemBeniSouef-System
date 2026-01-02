namespace ErpSystemBeniSouef.Core.Contract;

public interface ICollectionService
{
    Task<CollectorInstallmentsResultDto> GetCollectorInstallmentsAsync(int collectorId, DateTime date);
    Task<bool> SaveCollectionInstallmentsAsync(List<InstallmentCollectionDto> installments);
}