using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Core.DTOs.Reports.MonthlyCollectingDtos;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;

namespace ErpSystemBeniSouef.Service.ReportsServices
{
    public class CollectionService
    {
        private readonly IUnitOfWork _unit;

        public CollectionService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        // ------------------------------------
        // 1️⃣ Get Installments for printing page
        // ------------------------------------
        public async Task<List<MonthlyCollectionItemDto>> GetMonthlyInstallmentsAsync(int collectorId, DateTime month)
        {
            var installments = _unit.Repository<MonthlyInstallment>()
                .GetAllQueryable(
               x => x.Customer
            ).Where(x => x.CollectorId == collectorId &&
                     x.MonthDate == month &&
                     x.IsPaid == false).ToList();

            return installments.Select(x => new MonthlyCollectionItemDto
            {
                InstallmentId = x.Id,
                CustomerId = x.CustomerId,
                CustomerName = x.Customer.Name,
                CustomerPhone = x.Customer.MobileNumber,
                Area = x.Customer.SubArea.Name,
                WorkPlace = x.Customer.Address,
                InstallmentAmount = x.Amount
            }).ToList();
        }

        // ------------------------------------
        // تسليم التحصيل الشهري
        // ------------------------------------
        //public async Task<MonthlyCollectionResponseDto> SubmitMonthlyCollectionAsync(SubmitMonthlyCollectionRequestDto dto)
        //{
        //    var installments = await GetMonthlyInstallmentsAsync(dto.CollectorId, dto.Month);

        //    if (!installments.Any())
        //        throw new Exception("لا يوجد أقساط لهذا المحصل في هذا الشهر");

        //    // Create Monthly Collection Record
        //    var collection = new CollectionBatch
        //    {
        //        CollectorId = dto.CollectorId,
        //        MonthDate = dto.Month,
        //      //  CarriedOverAmount = installments.,
        //        TotalAssignedAmount = installments.Sum(x => x.InstallmentAmount),
        //        CreatedDate = DateTime.Now
        //    };

        //     _unit.Repository<CollectionBatch>().Add(collection);
        //    await _unit.CompleteAsync();
        //    // Create Items
        //    foreach (var item in installments)
        //    {
        //         _unit.Repository<MonthlyInstallment>().
        //            Add(new MonthlyInstallment
        //        {
        //           // b = collection.Id,
        //            Id = item.InstallmentId,
        //            Amount = item.InstallmentAmount
        //        });

        //        // Update installment → mark as collected
        //        var inst = await _unit.Installments.GetByIdAsync(item.InstallmentId);
        //        inst.IsPaid = true;
        //        inst.PaidAt = DateTime.Now;
        //    }

        //    await _unit.CommitAsync();

        //    // Return response DTO
        //    return new MonthlyCollectionResponseDto
        //    {
        //        MonthlyCollectionId = collection.Id,
        //        CollectorId = dto.CollectorId,
        //        CollectorName = await _unit.Collectors.GetNameById(dto.CollectorId),
        //        MonthName = CultureInfo.GetCultureInfo("ar-EG").DateTimeFormat.GetMonthName(dto.Month),
        //        TotalAmount = collection.TotalAmount,
        //        Items = installments
        //    };
        //}
        //public async Task<bool> AddPaymentAsync(AddCollectionEntryDto dto)
        //{
        //    using var trx = await _unit.BeginTransactionAsync();

        //    var inst = await _unit.Repository<MonthlyInstallment>().GetByIdAsync(dto.InstallmentId);
        //    if (inst == null) throw new Exception("Installment not found");

        //    var entry = new CollectionEntry
        //    {
        //        MonthlyInstallmentId = inst.Id,
        //        CollectionBatchId = inst.CollectionBatchId,
        //        CollectionDate = DateTime.Now,
        //        PaidAmount = dto.PaidAmount
        //    };

        //    await _unit.Repository<CollectionEntry>().AddAsync(entry);

        //    inst.CollectedAmount += dto.PaidAmount;

        //    // If fully paid → mark completed
        //    if (inst.CollectedAmount >= inst.Amount)
        //    {
        //        inst.IsDelayed = false;
        //    }
        //    else
        //    {
        //        // delayed — create next month installment
        //        inst.IsDelayed = true;

        //        await _unit.Repository<MonthlyInstallment>().AddAsync(new MonthlyInstallment
        //        {
        //            InvoiceId = inst.InvoiceId,
        //            CustomerId = inst.CustomerId,
        //            CollectorId = inst.CollectorId,
        //            MonthDate = inst.MonthDate.AddMonths(1),
        //            Amount = inst.Amount - inst.CollectedAmount,
        //            CollectedAmount = 0,
        //            IsDelayed = true
        //        });
        //    }

        //    await _unit.CommitAsync();
        //    await trx.CommitAsync();

        //    return true;
        //}
    }
    }
