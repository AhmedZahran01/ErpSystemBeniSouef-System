namespace ErpSystemBeniSouef.Service.CollectionServices;

public class CollectionService(IUnitOfWork unitOfWork) : ICollectionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CollectorInstallmentsResultDto> GetCollectorInstallmentsAsync(int collectorId, DateTime date)
    {
        var query = _unitOfWork.Repository<MonthlyInstallment>()
            .GetAllQueryable(x => x.Customer, x => x.Invoice)
            .Where(x => x.CollectorId == collectorId && x.MonthDate.Month == date.Month);

        var installments = await query
            .Select(x => new InstallmentCollectionDto
            {
                InstallmentId = x.Id,
                CustomerName = x.Customer.Name,
                Amount = x.Amount,
                CollectedAmount = x.CollectedAmount,
                IsPaid = x.IsPaid,
                IsDelayed = x.IsDelayed
            })
            .ToListAsync();

        if (!installments.Any())
        {
            return new CollectorInstallmentsResultDto
            {
                CollectorName = string.Empty,
                TotalAmount = 0,
                Installments = []
            };
        }

        var collectorName = await query.Select(x => x.Collector.Name).FirstOrDefaultAsync() ?? string.Empty;

        var totalAmount = installments.Sum(x => x.Amount);

        var response = new CollectorInstallmentsResultDto
        {
            CollectorName = collectorName,
            TotalAmount = totalAmount,
            Installments = installments
        };

        return response;
    }

    public async Task<bool> SaveCollectionInstallmentsAsync(List<InstallmentCollectionDto> installments)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var installment in installments)
            {
                var entity = await _unitOfWork.Repository<MonthlyInstallment>()
                    .GetByIdAsync(installment.InstallmentId);

                if (entity is null) continue;

                entity.CollectedAmount = installment.CollectedAmount;
                entity.IsPaid = entity.CollectedAmount >= entity.Amount;

                _unitOfWork.Repository<MonthlyInstallment>().Update(entity);
            }

            await _unitOfWork.CompleteAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}