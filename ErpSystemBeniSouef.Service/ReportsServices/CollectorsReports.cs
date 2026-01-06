namespace ErpSystemBeniSouef.Service.ReportsServices;

public class CollectorsReports(IUnitOfWork unitOfWork) : ICollectorsReports
{

    #region Constructor Region
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #region Get Installment Sales Report  Region

    // المبيعات تقسيط 
    public async Task<(List<InstallmentReportDto>, decimal totalDeposits, Byte[] fileContent)> GetInstallmentSalesReportAsync(
        DateTime fromDate,
        DateTime toDate,
        int collectorId)
    {
        var monthlyInstallments = _unitOfWork.Repository<MonthlyInstallment>()
            .GetAllQueryable(x => x.Customer)
            .Include(x => x.Invoice!)
            .ThenInclude(i => i.Items!)
            .ThenInclude(i => i.Product)
            .Include(x => x.Invoice!)
            .ThenInclude(i => i.Installments!)
            .Where(x => x.CollectorId == collectorId &&
                    x.MonthDate.Date >= fromDate.Date &&
                    x.MonthDate.Date <= toDate.Date)
            .AsQueryable();

        var result = await monthlyInstallments
            .GroupBy(x => x.InvoiceId)
            .Select(invoice => new InstallmentReportDto
            {
                InvoiceDate = invoice.First().CreatedDate,
                CustomerName = invoice.First().Customer.Name,
                CustomerNumber = invoice.First().Customer.CustomerNumber,
                Deposit = invoice.First().Customer.Deposit,
                Plans = string.Join("+", invoice.First().Invoice.Installments!.Select(x => $"{x.Amount} * {x.NumberOfMonths}")),
                Items = string.Join("+", invoice.First().Invoice.Items!.Select(x => $"{x.Quantity} {x.Product.ProductName} ({x.Total})")),
            })
            .ToListAsync();

        var totalDeposits = result.Sum(x => x.Deposit);

        var file = await PrintDeposits(result);

        return (result, totalDeposits, file);
    }

    #endregion

    #region Get Representative Covenants Region

    // نسبة المرتجعات
    public async Task<(List<CovenantReportRowDto>, Byte[] fileContent, decimal totalCommision)> GetRepresentativeCovenantsAsync(
        DateTime fromDate,
        DateTime toDate,
        int representativeId)
    {
        var query = _unitOfWork.Repository<Covenant>()
            .GetAllQueryable(
                x => x.Representative,
                x => x.Customer,
                x => x.CovenantProducts,
                x => x.CovenantProducts.Select(p => p.Product)
            )
            .Where(x => x.RepresentativeId == representativeId &&
                    x.MonthDate.Date >= fromDate.Date &&
                    x.MonthDate.Date <= toDate.Date);

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

        var totalCommision = result.Sum(c => c.TotalCommisionRate);

        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("CovenantReport");

        var headers = new string[] { "Customer Number", "Customer Name", "Product",
            "Quantity", "CommisionRate", "TotalCommisionRate"};

        for (int i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).SetValue(headers[i]);

        var headerRange = sheet.Range(1, 1, 1, headers.Length);
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Font.SetBold();
        headerRange.Style.Font.SetFontSize(14);
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        for (int rowIndex = 0; rowIndex < result.Count; rowIndex++)
        {
            var s = result[rowIndex];
            int excelRow = rowIndex + 2;

            sheet.Cell(excelRow, 1).SetValue(s.CustomerNumber);
            sheet.Cell(excelRow, 2).SetValue(s.CustomerName);
            sheet.Cell(excelRow, 3).SetValue(s.ProductName);
            sheet.Cell(excelRow, 4).SetValue(s.Quantity);
            sheet.Cell(excelRow, 5).SetValue(s.CommisionRate);
            sheet.Cell(excelRow, 6).SetValue(s.TotalCommisionRate);
        }

        sheet.Columns().AdjustToContents();
        sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
        sheet.CellsUsed().Style.Font.SetFontSize(12);

        await using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return (result, stream.ToArray(), totalCommision);
    }

    #endregion

    #region Get Representative Cash Invoices Region

    // المبيعات كاش
    public async Task<(List<CashInvoicesReportDto>, Byte[] fileContent, decimal totalCash)> GetRepresentativeCashInvoicesAsync(
        DateTime fromDate,
        DateTime toDate,
        int representativeId)
    {
        var query = _unitOfWork.Repository<CashCstomerInvoice>()
            .GetAllQueryable(x => x.Representative, x => x.Items)
            .Where(x => x.Representative.Id == representativeId);

        var cashInvoicesToPrint = await query
            .Select(x => new PrintCashInvoicesDto
            {
                Representative = x.Representative.Name,
                MainArea = x.SubArea.mainRegions!.Name,
                SubArea = x.SubArea.Name,
                InvoiceDate = x.InvoiceDate,
                TotalAmount = x.TotalAmount ?? 0
            })
            .ToListAsync();

        var cashInvoicesToTable = await query
            .SelectMany(c => c.Items.Select(cp => new CashInvoicesReportDto
            {
                ProductName = cp.Product.ProductName,
                ProductType = cp.Product.Category.Name,
                Quentity = cp.Quantity,
                UnitPrice = cp.Price,
            }))
            .ToListAsync();

        var totalCash = cashInvoicesToPrint.Sum(c => c.TotalAmount);

        var file = await PrintcashInvoices(cashInvoicesToPrint);

        return (cashInvoicesToTable, file, totalCash);
    }

    #endregion

    #region Get All Items Installment Sales Report Region

    // نسبة المندبة الجديدة 
    public async Task<(List<RepresentativeCommissionReportDto>, decimal totalCommissionPercentage, Byte[] fileContent)> GetAllItemsInstallmentSalesReportAsync(
        DateTime fromDate,
        DateTime toDate,
        int representativeId)
    {
        var result = await _unitOfWork.Repository<Commission>()
            .GetAllQueryable()
            .Where(x =>
                x.RepresentativeId == representativeId
                && x.MonthDate.Date >= fromDate.Date
                && x.MonthDate.Date <= toDate.Date
            )
            .Select(c => new RepresentativeCommissionReportDto
            {
                ProductName = c.Product.ProductName,
                CommissionAmount = c.CommissionAmount,
                QuantitySold = c.InvoiceItem.Quantity,
                TotalPercentage = c.CommissionAmount * c.InvoiceItem.Quantity
            })
            .ToListAsync();

        var totalCommissionPercentage = result.Sum(x => x.TotalPercentage);

        var file = await PrintCommissions(result);

        return (result, totalCommissionPercentage, file);
    }

    #endregion

    #region Common

    public async Task<RepresentativeReportSummaryDto> GetRepresentativeSummaryAsync(
            DateTime fromDate, DateTime toDate, int collectorId)
    {
        var (_, totalDeposits, _) =
            await GetInstallmentSalesReportAsync(fromDate, toDate, collectorId);

        var (_, _, totalCash) =
            await GetRepresentativeCashInvoicesAsync(fromDate, toDate, collectorId);

        var (_, totalCommissionPercentage, _) =
            await GetAllItemsInstallmentSalesReportAsync(fromDate, toDate, collectorId);

        var netCommission = totalCommissionPercentage + totalCash + totalDeposits;

        return new RepresentativeReportSummaryDto
        {
            TotalDeposits = totalDeposits,
            TotalCash = totalCash,
            TotalRepresentativeCommission = totalCommissionPercentage,
            NetCommission = netCommission
        };
    }

    #endregion


    #region Print Region

    private async Task<Byte[]> PrintDeposits(List<InstallmentReportDto> installments)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("المقدمات");

        var headers = new string[] { "اسم العميل", "المقدم"};

        for (int i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).SetValue(headers[i]);

        var headerRange = sheet.Range(1, 1, 1, headers.Length);
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Font.SetBold();
        headerRange.Style.Font.SetFontSize(14);
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        for (int rowIndex = 0; rowIndex < installments.Count; rowIndex++)
        {
            var s = installments[rowIndex];
            int excelRow = rowIndex + 2;

            sheet.Cell(excelRow, 1).SetValue(s.CustomerName);
            sheet.Cell(excelRow, 2).SetValue(s.Deposit);
        }

        sheet.Columns().AdjustToContents();
        sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
        sheet.CellsUsed().Style.Font.SetFontSize(12);

        await using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return (stream.ToArray());
    }

    private async Task<Byte[]> PrintCommissions(List<RepresentativeCommissionReportDto> representativeCommissions)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("نسبة المندبة");

        var headers = new string[] { "اسم السلعة ", "نسبة السلعة", "العدد المباع", "النسبة الكلية لكل سلعة" };

        for (int i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).SetValue(headers[i]);

        var headerRange = sheet.Range(1, 1, 1, headers.Length);
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Font.SetBold();
        headerRange.Style.Font.SetFontSize(14);
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        for (int rowIndex = 0; rowIndex < representativeCommissions.Count; rowIndex++)
        {
            var s = representativeCommissions[rowIndex];
            int excelRow = rowIndex + 2;

            sheet.Cell(excelRow, 1).SetValue(s.ProductName);
            sheet.Cell(excelRow, 2).SetValue(s.CommissionAmount);
            sheet.Cell(excelRow, 3).SetValue(s.QuantitySold);
            sheet.Cell(excelRow, 4).SetValue(s.TotalPercentage);
        }

        sheet.Columns().AdjustToContents();
        sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
        sheet.CellsUsed().Style.Font.SetFontSize(12);

        await using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return (stream.ToArray());
    }

    private async Task<Byte[]> PrintcashInvoices(List<PrintCashInvoicesDto> cashInvoicesToPrint)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("إجمالي المقدمات");

        var headers = new string[] { "المندوب", "المنطقة الرئيسية", "المنطقة الفرعية", "تاريخ الفاتورة", "المبلغ الإجمالي" };

        for (int i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).SetValue(headers[i]);

        var headerRange = sheet.Range(1, 1, 1, headers.Length);
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Font.SetBold();
        headerRange.Style.Font.SetFontSize(14);
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        for (int rowIndex = 0; rowIndex < cashInvoicesToPrint.Count; rowIndex++)
        {
            var s = cashInvoicesToPrint[rowIndex];
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

        return (stream.ToArray());
    }

    #endregion


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

    //     _unitOfWork.Repository<CollectionBatch>().Add(collection);
    //    await _unitOfWork.CompleteAsync();
    //    // Create Items
    //    foreach (var item in installments)
    //    {
    //         _unitOfWork.Repository<MonthlyInstallment>().
    //            Add(new MonthlyInstallment
    //        {
    //           // b = collection.Id,
    //            Id = item.InstallmentId,
    //            Amount = item.InstallmentAmount
    //        });

    //        // Update installment → mark as collected
    //        var inst = await _unitOfWork.Installments.GetByIdAsync(item.InstallmentId);
    //        inst.IsPaid = true;
    //        inst.PaidAt = DateTime.Now;
    //    }

    //    await _unitOfWork.CommitAsync();

    //    // Return response DTO
    //    return new MonthlyCollectionResponseDto
    //    {
    //        MonthlyCollectionId = collection.Id,
    //        CollectorId = dto.CollectorId,
    //        CollectorName = await _unitOfWork.Collectors.GetNameById(dto.CollectorId),
    //        MonthName = CultureInfo.GetCultureInfo("ar-EG").DateTimeFormat.GetMonthName(dto.Month),
    //        TotalAmount = collection.TotalAmount,
    //        Items = installments
    //    };
    //}
    //public async Task<bool> AddPaymentAsync(AddCollectionEntryDto dto)
    //{
    //    using var trx = await _unitOfWork.BeginTransactionAsync();

    //    var inst = await _unitOfWork.Repository<MonthlyInstallment>().GetByIdAsync(dto.InstallmentId);
    //    if (inst == null) throw new Exception("Installment not found");

    //    var entry = new CollectionEntry
    //    {
    //        MonthlyInstallmentId = inst.Id,
    //        CollectionBatchId = inst.CollectionBatchId,
    //        CollectionDate = DateTime.Now,
    //        PaidAmount = dto.PaidAmount
    //    };

    //    await _unitOfWork.Repository<CollectionEntry>().AddAsync(entry);

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

    //        await _unitOfWork.Repository<MonthlyInstallment>().AddAsync(new MonthlyInstallment
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

    //    await _unitOfWork.CommitAsync();
    //    await trx.CommitAsync();

    //    return true;
    //}
}
