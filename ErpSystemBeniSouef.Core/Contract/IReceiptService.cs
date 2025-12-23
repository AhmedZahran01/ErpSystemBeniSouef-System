namespace ErpSystemBeniSouef.Core.Contract;

public interface IReceiptService
{
    Task<(List<GetAllReceiptsDto>, Byte[])> GetAllReceiptsAsync(int? mainAraeId, int? fromCustomerNumber, int? toCustomerNumber);
    Task<(List<GetAllReceiptsDto>, Byte[])> GetMonthlyReceiptsAsync(DateTime month, int? mainAraeId, int? subAreaId);
    Task<(List<GetAllReceiptsDto>, Byte[])> GetCollectorReceiptsAsync(DateTime? month, int collectorId);
    Task<(List<GetAllReceiptsDto>, Byte[])> GetCustomerReceiptsAsync(int collectorNumber);
}