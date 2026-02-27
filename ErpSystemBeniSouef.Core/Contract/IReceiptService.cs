namespace ErpSystemBeniSouef.Core.Contract;

public interface IReceiptService
{
    Task<List<GetAllReceiptsDto>> GetAllReceiptsAsync(int? mainAraeId = null, int? fromCustomerNumber = null, int? toCustomerNumber = null);
    //Task<(List<GetAllReceiptsDto>, Byte[])> GetAllReceiptsAsync(int? mainAraeId = null, int? fromCustomerNumber = null, int? toCustomerNumber = null);
    Task<List<GetAllReceiptsDto>> GetMonthlyReceiptsAsync(DateTime? month = null, int? mainAraeId = null, int? subAreaId = null);
    Task<List<GetAllReceiptsDto>> GetCollectorReceiptsAsync(DateTime? month, int? collectorId = null);
    Task<List<GetAllReceiptsDto>> GetCustomerReceiptsAsync(int collectorNumber);
}