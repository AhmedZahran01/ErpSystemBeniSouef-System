namespace ErpSystemBeniSouef.Core.Contract;

public interface IReceiptService
{
    Task<(List<GetAllReceiptsDto>, Byte[])> GetAllReceiptsAsync(int? mainAraeId);
}