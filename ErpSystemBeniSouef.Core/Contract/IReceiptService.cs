namespace ErpSystemBeniSouef.Core.Contract;

public interface IReceiptService
{
    Task<(List<GetAllReceiptsDto>, Byte[])> GetAllReceiptsAsync(int? mainAraeId);
    Task<(List<GetAllReceiptsDto>, Byte[])> GetMonthlyReceiptsAsync(DateTime month, int? mainAraeId, int? subAreaId);
}