namespace ErpSystemBeniSouef.Service.ReceiptServices;

public class ReceiptService(IUnitOfWork unitOfWork) : IReceiptService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<(List<GetAllReceiptsDto>, Byte[])> GetAllReceiptsAsync(int? mainAraeId = null, int? fromCustomerNumber = null, int? toCustomerNumber = null)
    {
        var query = _unitOfWork.Repository<MonthlyInstallment>()
            .GetAllQueryable()
            .Include(x => x.Customer)
            .ThenInclude(c => c.Representative)
            .Include(x => x.Customer)
            .ThenInclude(c => c.SubArea)
            .ThenInclude(sa => sa!.mainRegions)
            .Include(x => x.Collector)
            .Include(x => x.Invoice)
            .ThenInclude(i => i.Items)!
            .ThenInclude(it => it.Product)
            .Include(x => x.Invoice)
            .ThenInclude(i => i.Installments)
            .AsNoTracking()
            .AsQueryable();

        if (mainAraeId.HasValue)
            query = query.Where(x => x.Customer.SubArea!.MainAreaId == mainAraeId);

        if (fromCustomerNumber.HasValue)
            query = query.Where(x => x.Customer.CustomerNumber >= fromCustomerNumber);

        if (toCustomerNumber.HasValue)
            query = query.Where(x => x.Customer.CustomerNumber <= toCustomerNumber);

        var receipts = await query
            .GroupBy(x => x.InvoiceId)
            .Select(receipt => new GetAllReceiptsDto
            {
                MonthlyInstallmentId = receipt.First().Id,
                CustomerNumber = receipt.First().Customer.CustomerNumber,
                CustomerName = receipt.First().Customer.Name,
                MobileNumber = receipt.First().Customer.MobileNumber,
                Address = receipt.First().Customer.Address,
                NationlNumber = receipt.First().Customer.NationalNumber,
                Deposite = receipt.First().Customer.Deposit,
                //CollectorName = receipt.First().Collector.Name,
                RepresentativeName = receipt.First().Customer.Representative!.Name,
                AreaName = receipt.First().Customer.SubArea!.Name,
                FirstInvoiceDate = receipt.First().Customer.FirstInvoiceDate,
                InvoiceDate = receipt.First().Invoice.InvoiceDate,
                TotalPrice = receipt.First().Invoice.Items!.Sum(item => item.Quantity * item.Price),
                Items = string.Join("+", receipt.First().Invoice.Items!.Select(x => $"{x.Quantity} {x.Product.ProductName} ({x.Total})")),
                Plans = string.Join("+", receipt.First().Invoice.Installments!.Select(x => $"{x.NumberOfMonths} * {x.Amount}"))
            })
            .ToListAsync();

        var file = await PrintAllReceipts(receipts);

        return (receipts, file);
    }

    public async Task<(List<GetAllReceiptsDto>, byte[])> GetMonthlyReceiptsAsync(DateTime? month = null, int? mainAreaId = null, int? subAreaId = null)
    {
        var query = _unitOfWork.Repository<MonthlyInstallment>()
            .GetAllQueryable()
            .Include(x => x.Customer)
                .ThenInclude(c => c.SubArea)
                    .ThenInclude(sa => sa!.mainRegions)
            .Include(x => x.Invoice)
                .ThenInclude(i => i.Items)!
            .AsNoTracking();
            //.AsQueryable();

        // 🔹 فلترة الشهر
        if (month.HasValue)
        {
            query = query.Where(x =>
                x.MonthDate.Year == month.Value.Year &&
                x.MonthDate.Month == month.Value.Month);
        }

        // 🔹 فلترة المنطقة الرئيسية
        if (mainAreaId.HasValue)
        {
            query = query.Where(x =>
                x.Customer.SubArea!.MainAreaId == mainAreaId.Value);
        }

        // 🔹 فلترة المنطقة الفرعية
        if (subAreaId.HasValue)
        {
            query = query.Where(x =>
                x.Customer.SubAreaId == subAreaId.Value);
        }

        var receipts = await query.Select(receipt => new GetAllReceiptsDto
        {
            CustomerNumber = receipt.Customer.CustomerNumber,
            CustomerName = receipt.Customer.Name,
            Address = receipt.Customer.Address,
            AreaName = receipt.Customer.SubArea!.Name,
            TotalPrice = receipt.Invoice.Items!
                .Sum(item => item.Quantity * item.Price)
        })
        .ToListAsync();

        var file = await PrintMonthlyReceipts(receipts);

        return (receipts, file);
    }


    public async Task<(List<GetAllReceiptsDto>, Byte[])> GetCollectorReceiptsAsync(DateTime? month, int collectorId)
    {
        var query = _unitOfWork.Repository<MonthlyInstallment>()
            .GetAllQueryable()
            .Include(x => x.Customer)
            .ThenInclude(c => c.SubArea)
            .ThenInclude(sa => sa!.mainRegions)
            .Include(x => x.Invoice)
            .ThenInclude(i => i.Items)!
            .AsNoTracking()
            .AsQueryable();

        query = query.Where(x => x.Collector.Id == collectorId);

        if (month.HasValue)
            query = query.Where(x => x.MonthDate.Year == month.Value.Year && x.MonthDate.Month == month.Value.Month);

        var receipts = await query.Select(receipt => new GetAllReceiptsDto
        {
            CustomerNumber = receipt.Customer.CustomerNumber,
            CustomerName = receipt.Customer.Name,
            Address = receipt.Customer.Address,
            AreaName = $"{receipt.Customer.SubArea!.mainRegions!.Name}/{receipt.Customer.SubArea!.Name}",
            TotalPrice = receipt.Invoice.Items!.Sum(item => item.Quantity * item.Price)
        })
        .ToListAsync();

        var file = await PrintMonthlyReceipts(receipts);

        return (receipts, file);
    }

    public async Task<(List<GetAllReceiptsDto>, Byte[])> GetCustomerReceiptsAsync(int customerNumber)
    {
        var customerExists = _unitOfWork.Repository<Customer>()
            .GetByCondionAndInclide(x => x.CustomerNumber == customerNumber) ?? throw new Exception("رقم العميل غير صحيح أو غير موجود");

        var query = _unitOfWork.Repository<MonthlyInstallment>()
            .GetAllQueryable()
            .Include(x => x.Customer)
            .ThenInclude(c => c.SubArea)
            .ThenInclude(sa => sa!.mainRegions)
            .Include(x => x.Invoice)
            .ThenInclude(i => i.Items)!
            .AsNoTracking()
            .AsQueryable();

        query = query.Where(x => x.Customer.CustomerNumber == customerNumber);

        var receipts = await query
            .GroupBy(x => x.CustomerId)
            .Select(receipt => new GetAllReceiptsDto
            {
                CustomerNumber = receipt.First().Customer.CustomerNumber,
                CustomerName = receipt.First().Customer.Name,
                Address = receipt.First().Customer.Address,
                AreaName = receipt.First().Customer.SubArea!.Name,
                TotalPrice = receipt.First().Invoice.Items!.Sum(item => item.Quantity * item.Price)
            })
            .ToListAsync();

        var file = await PrintMonthlyReceipts(receipts);

        return (receipts, file);
    }

    private async Task<Byte[]> PrintAllReceipts(List<GetAllReceiptsDto> receipts)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("All Receipts");

        var headers = new string[] { "Serial number", "Customer number", "Customer name", "Mobile number", "Address",
                "Nationl number", "Deposite", "Collector name", "Representative name", "Area", "First invoice date",
                "Invoice date", "Items", "Plans"};

        for (int i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).SetValue(headers[i]);

        var headerRange = sheet.Range(1, 1, 1, headers.Length);
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Font.SetBold();
        headerRange.Style.Font.SetFontSize(14);
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        for (int rowIndex = 0; rowIndex < receipts.Count; rowIndex++)
        {
            var r = receipts[rowIndex];
            int excelRow = rowIndex + 2;

            sheet.Cell(excelRow, 1).SetValue(r.MonthlyInstallmentId);
            sheet.Cell(excelRow, 2).SetValue(r.CustomerNumber);
            sheet.Cell(excelRow, 3).SetValue(r.CustomerName);
            sheet.Cell(excelRow, 4).SetValue(r.MobileNumber);
            sheet.Cell(excelRow, 5).SetValue(r.Address);
            sheet.Cell(excelRow, 6).SetValue(r.NationlNumber);
            sheet.Cell(excelRow, 7).SetValue(r.Deposite);
            //sheet.Cell(excelRow, 8).SetValue(r.CollectorName);
            sheet.Cell(excelRow, 9).SetValue(r.RepresentativeName);
            sheet.Cell(excelRow, 10).SetValue(r.AreaName);
            sheet.Cell(excelRow, 11).SetValue(r.InvoiceDate);
            sheet.Cell(excelRow, 12).SetValue(r.FirstInvoiceDate);
            sheet.Cell(excelRow, 13).SetValue(r.Items);
            sheet.Cell(excelRow, 14).SetValue(r.TotalPrice);
            sheet.Cell(excelRow, 15).SetValue(r.Plans);
        }

        sheet.Columns().AdjustToContents();
        sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
        sheet.CellsUsed().Style.Font.SetFontSize(12);

        await using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
    private async Task<Byte[]> PrintMonthlyReceipts(List<GetAllReceiptsDto> receipts)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("Monthly Receipts");

        var headers = new string[] {"Customer number", "Customer name", "Address", "Area", "Total price"};

        for (int i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).SetValue(headers[i]);

        var headerRange = sheet.Range(1, 1, 1, headers.Length);
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Font.SetBold();
        headerRange.Style.Font.SetFontSize(14);
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        for (int rowIndex = 0; rowIndex < receipts.Count; rowIndex++)
        {
            var r = receipts[rowIndex];
            int excelRow = rowIndex + 2;

            sheet.Cell(excelRow, 1).SetValue(r.CustomerNumber);
            sheet.Cell(excelRow, 2).SetValue(r.CustomerName);
            sheet.Cell(excelRow, 3).SetValue(r.Address);
            sheet.Cell(excelRow, 4).SetValue(r.AreaName);
            sheet.Cell(excelRow, 5).SetValue(r.TotalPrice);
        }

        sheet.Columns().AdjustToContents();
        sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
        sheet.CellsUsed().Style.Font.SetFontSize(12);

        await using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}