using Castle.Core.Resource;
using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.DTOs.CustomerInvoiceDtos.GetAllDetailsForCustomerInvoiceDtos;
using ErpSystemBeniSouef.Core.DTOs.Reports;
using ErpSystemBeniSouef.Core.DTOs.Reports.MonthlyCollectingDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Entities.CovenantModels;
using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;
using ErpSystemBeniSouef.Core.Enum;
using ErpSystemBeniSouef.Infrastructer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<InstallmentReportDto>> GetInstallmentSalesReportAsync(
            DateTime fromDate,
            DateTime toDate,
            int collectorId)
        {
            var monthlyInstallments = await _unit.Repository<MonthlyInstallment>()
                .GetAllQueryable(x => x.Customer, x => x.Invoice, x => x.Collector)
                .ToListAsync();

            var invoiceIds = monthlyInstallments.Select(m => m.InvoiceId).Distinct().ToList();

            var invoicesQuery = _unit.Repository<CustomerInvoice>()
                .GetAllQueryable(x => x.Customer!, x => x.Installments!, x => x.Items!)
                .Where(x => invoiceIds.Contains(x.Id));

            invoicesQuery = invoicesQuery.Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate);

            var invoices = await invoicesQuery.ToListAsync();

            var result = invoices.Select(invoice => new InstallmentReportDto
            {
                InvoiceDate = invoice.InvoiceDate,
                CustomerName = invoice.Customer.Name,
                TotalAmount = invoice.TotalAmount,
                Deposit = invoice.Customer.Deposit,

                Plans = invoice.Installments?.Select(p => new InstallmentPlanDto
                {
                    InstallmentAmount = p.Amount,
                    Months = p.NumberOfMonths
                }).ToList() ?? [],

                Items = invoice.Items?.Select(item => new CustomerInvoiceItemDto
                {
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    Total = item.Total
                }).ToList() ?? []

            }).ToList();

            return result;
        }

        public async Task<List<CovenantReportRowDto>> GetRepresentativeCovenantsAsync(
            DateTime fromDate,
            DateTime toDate,
            int collectorId)
        {
            var query = _unit.Repository<Covenant>()
                .GetAllQueryable(
                    x => x.Representative,
                    x => x.Customer,
                    x => x.CovenantProducts,
                    x => x.CovenantProducts.Select(p => p.Product)
                )
                .Where(x =>
                    x.Representative.Id == collectorId &&
                    x.MonthDate >= fromDate &&
                    x.MonthDate <= toDate
                );

            var result = await query
                .SelectMany(c => c.CovenantProducts.Select(cp => new CovenantReportRowDto
                {
                    CustomerNumber = c.Customer.CustomerNumber,
                    CustomerName = c.Customer.Name,
                    ProductName = cp.Product.ProductName,
                    Quantity = cp.Amount,
                    CommisionRate = cp.Product.CommissionRate,
                    TotalCommisionRate = cp.Amount * cp.Product.CommissionRate,
                }))
                .ToListAsync();

            return result;
        }

        public async Task<List<CashInvoicesReportDto>> GetRepresentativeCashInvoicesAsync(
            DateTime fromDate,
            DateTime toDate,
            int collectorId)
        {
            var query = _unit.Repository<CashCstomerInvoice>()
                .GetAllQueryable(x => x.Representative, x => x.Items)
                .Where(x =>
                    x.Representative.Id == collectorId &&
                    x.InvoiceDate >= fromDate &&
                    x.InvoiceDate <= toDate
                );

            var result = await query
                .SelectMany(c => c.Items.Select(cp => new CashInvoicesReportDto
                {
                    ProductName = cp.Product.ProductName,
                    ProductType = cp.Product.Category.Name,
                    Quentity = cp.Quantity,
                    UnitPrice = cp.Price,
                }))
                .ToListAsync();

            return result;
        }

        public async Task<List<RepresentativeCommissionReportDto>> GetAllItemsInstallmentSalesReportAsync(
            DateTime fromDate,
            DateTime toDate,
            int collectorId)
        {
            var result = await _unit.Repository<Commission>()
                .GetAllQueryable(x => x.Representative, x => x.Product, x => x.InvoiceItem, x => x.Invoice)
                .Where(x =>
                    x.Representative.Id == collectorId &&
                    x.MonthDate >= fromDate &&
                    x.MonthDate <= toDate &&
                    x.Type == CommissionType.Earn)
                    .Select(c => new RepresentativeCommissionReportDto
                    {
                        ProductName = c.Product.ProductName,
                        CommissionAmount = c.CommissionAmount,
                        QuantitySold = c.InvoiceItem.Quantity,
                        TotalPercentage = c.CommissionAmount * c.InvoiceItem.Quantity
                    })
                    .ToListAsync();

            return result;
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