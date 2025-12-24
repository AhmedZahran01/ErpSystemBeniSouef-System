namespace ErpSystemBeniSouef.Service.CollectorServices;

public class CollectionService(IUnitOfWork unitOfWork) : ICollectionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    public async Task<List<MonthlyCollectionItemDto>> GetMonthlyInstallmentsAsync(int collectorId, DateTime month)
    {
        var installments = _unitOfWork.Repository<MonthlyInstallment>()
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
        int representativeId)
    {
        var monthlyInstallments = await _unitOfWork.Repository<MonthlyInstallment>()
            .GetAllQueryable(x => x.Customer)
            .Include(x => x.Invoice!)
            .ThenInclude(i => i.Items!)
            .ThenInclude(i => i.Product)
            .Include(x => x.Invoice!)
            .ThenInclude(i => i.Installments!)
            .Where(x => x.CollectorId == representativeId &&
                    x.MonthDate.Date >= fromDate.Date &&
                    x.MonthDate.Date <= toDate.Date)
            .ToListAsync();

        var result = monthlyInstallments.Select(invoice => new InstallmentReportDto
        {
            InvoiceDate = invoice.MonthDate,
            CustomerName = invoice.Customer.Name,
            TotalAmount = invoice.Invoice.TotalAmount,
            Deposit = invoice.Customer.Deposit,
            Plans = string.Join("+", invoice.Invoice.Installments!.Select(x => $"{x.Amount} * {x.NumberOfMonths}")),
            Items = string.Join("+", invoice.Invoice.Items!.Select(x => $"{x.Quantity} {x.Product.ProductName} ({x.Total})")),
        }).ToList();

        return result;
    }

    public async Task<(List<CovenantReportRowDto>, Byte[] FileContent, decimal totalCommision)> GetRepresentativeCovenantsAsync(
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

    public async Task<(List<CashInvoicesReportDto>, Byte[] FileContent, decimal totalCash)> GetRepresentativeCashInvoicesAsync(
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

        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("Total Cash");

        var headers = new string[] { "Representative", "Mainarea", "Subarea", "Invoice date", "Total amount" };

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

        return (cashInvoicesToTable, stream.ToArray(), totalCash);
    }

    public async Task<List<RepresentativeCommissionReportDto>> GetAllItemsInstallmentSalesReportAsync(
        DateTime fromDate,
        DateTime toDate,
        int representativeId)
    {
        var result = await _unitOfWork.Repository<Commission>()
            .GetAllQueryable()
            .Include(x => x.Representative)
            .Include(x => x.Product)
            .Include(x => x.InvoiceItem)
            .Where(x =>
                x.RepresentativeId == representativeId
                && x.MonthDate.Date >= fromDate.Date
                && x.MonthDate.Date <= toDate.Date)
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
        var customers = await _unitOfWork.Repository<Customer>()
            .GetAllQueryable(x => x.Representative!, x => x.Collector!, x => x.SubArea!, x => x.Invoices!)
            .Where(x =>
                x.RepresentativeId == representativeId
                && x.FirstInvoiceDate.Date >= fromDate.Date
                && x.FirstInvoiceDate.Date <= toDate.Date)
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

        var headers = new string[] { "Customer Name", "Deposit", "TotalInvoices", "NetAmount " };

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
