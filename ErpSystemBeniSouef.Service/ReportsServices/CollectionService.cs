using Castle.Core.Resource;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
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
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        public async Task<(Byte[] FileContent, decimal totalDeposits)> PrintCustomersAccountAsync(
            DateTime fromDate,
            DateTime toDate,
            int representativeId)
        {
            var customers = await _unit.Repository<Customer>()
                .GetAllQueryable(x => x.Representative!, x => x.Collector!, x => x.SubArea!, x => x.Invoices!)
                .Where(x => x.Representative!.Id == representativeId)
                .Select(c => new CustomerAccountDto
                {
                    CustomerName = c.Name,
                    Deposit = c.Deposit,
                    TotalInvoices = c.Invoices!
                        .Where(i => i.InvoiceDate >= fromDate && i.InvoiceDate <= toDate)
                        .Sum(i => (decimal?)i.TotalAmount) ?? 0
                })
                .ToListAsync();

            customers.ForEach(c => c.NetAmount = c.TotalInvoices - c.Deposit);

            var totalDeposits = customers.Sum(c => c.Deposit);

            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet("customers");

            var headers = new string[] { "Customer Name", "Deposit", "TotalInvoices", "NetAmount "};

            for (int i = 0; i < headers.Length; i++)
                sheet.Cell(1, i + 1).SetValue(headers[i]);

            var headerRange = sheet.Range(1, 1, 1, headers.Length);
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Font.SetBold();
            headerRange.Style.Font.SetFontSize(14);
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            for (int rowIndex = 0; rowIndex < customers.Count; rowIndex++)
            {
                var s = customers[rowIndex];
                int excelRow = rowIndex + 2;

                sheet.Cell(excelRow, 1).SetValue(s.CustomerName);
                sheet.Cell(excelRow, 2).SetValue(s.Deposit);
                sheet.Cell(excelRow, 3).SetValue(s.TotalInvoices);
                sheet.Cell(excelRow, 4).SetValue(s.NetAmount);
            }

            sheet.Columns().AdjustToContents();
            sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
            sheet.CellsUsed().Style.Font.SetFontSize(12);

            await using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return (stream.ToArray(), totalDeposits);
        }

        public async Task<(Byte[] FileContent, decimal totalCash)> PrintCashInvoicesAsync(
            DateTime fromDate,
            DateTime toDate,
            int representativeId)
        {
            var customers = await _unit.Repository<CashCstomerInvoice>()
                .GetAllQueryable(x => x.Representative!, x => x.Items!, x => x.SubArea)
                .Where(x => x.Representative!.Id == representativeId && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                .Select(x => new PrintCashInvoicesDto
                {
                    Representative = x.Representative.Name,
                    MainArea = x.SubArea.mainRegions!.Name,
                    SubArea = x.SubArea.Name,
                    InvoiceDate = x.InvoiceDate,
                    TotalAmount = x.TotalAmount ?? 0
                })
                .ToListAsync();

            var totalCash = customers.Sum(c => c.TotalAmount);

            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet("Total Cash");

            var headers = new string[] { "Representative", "Mainarea", "Subarea", "Invoice date", "Total amount"};

            for (int i = 0; i < headers.Length; i++)
                sheet.Cell(1, i + 1).SetValue(headers[i]);

            var headerRange = sheet.Range(1, 1, 1, headers.Length);
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Font.SetBold();
            headerRange.Style.Font.SetFontSize(14);
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            for (int rowIndex = 0; rowIndex < customers.Count; rowIndex++)
            {
                var s = customers[rowIndex];
                int excelRow = rowIndex + 2;

                sheet.Cell(excelRow, 1).SetValue(s.Representative);
                sheet.Cell(excelRow, 2).SetValue(s.MainArea);
                sheet.Cell(excelRow, 3).SetValue(s.SubArea);
                sheet.Cell(excelRow, 4).SetValue(s.InvoiceDate);
                sheet.Cell(excelRow, 4).SetValue(s.TotalAmount);
            }

            sheet.Columns().AdjustToContents();
            sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
            sheet.CellsUsed().Style.Font.SetFontSize(12);

            await using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return (stream.ToArray(), totalCash);
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