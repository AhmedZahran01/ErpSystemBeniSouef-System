namespace ErpSystemBeniSouef.Service.ReceiptServices;

public class ReceiptService(IUnitOfWork unitOfWork) : IReceiptService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<(List<GetAllReceiptsDto>, Byte[])> GetAllReceiptsAsync(int? mainAraeId)
    {
        var query = _unitOfWork.Repository<MonthlyInstallment>()
            .GetAllQueryable(
                x => x.Customer,
                x => x.Customer.Representative!,
                x => x.Customer.SubArea!,
                x => x.Customer.SubArea!.mainRegions!,
                x => x.Collector,
                x => x.Invoice,
                x => x.Invoice.Items!,
                x => x.Invoice.Items!.Select(i => i.Product),
                x => x.Invoice.Installments!
            );

        if (mainAraeId.HasValue)
            query = query.Where(x => x.Customer.SubArea!.MainAreaId == mainAraeId);

        var receipts = await query.Select(receipt => new GetAllReceiptsDto
        {
            MonthlyInstallmentId = receipt.Id,
            CustomerNumber = receipt.Customer.CustomerNumber,
            CustomerName = receipt.Customer.Name,
            MobileNumber = receipt.Customer.MobileNumber,
            Address = receipt.Customer.Address,
            NationlNumber = receipt.Customer.NationalNumber,
            Deposite = receipt.Customer.Deposit,
            CollectorName = receipt.Collector.Name,
            RepresentativeName = receipt.Customer.Representative!.Name,
            AreaName = receipt.Customer.SubArea!.Name,
            InvoiceDate = receipt.Invoice.InvoiceDate,
            MonthDate = receipt.MonthDate,
            Items = receipt.Invoice.Items!.Select(item => new CustomerInvoiceItemDto
            {
                ProductName = item.Product.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.Price,
                Total = item.Quantity * item.Price
            }).ToList(),
            Plans = receipt.Invoice.Installments!.Select(plan => new InstallmentPlanDto
            {
                InstallmentAmount = plan.Amount,
                Months = plan.NumberOfMonths
            }).ToList(),
            TotalPrice = receipt.Invoice.Items!.Sum(item => item.Quantity * item.Price) 
        })
        .AsNoTracking()
        .ToListAsync();

        var file = await PrintReceipts(receipts);

        return (receipts, file);
    }

    private async Task<Byte[]> PrintReceipts(List<GetAllReceiptsDto> receipts)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("All Receipts");

        var headers = new string[] { "Serial number", "Customer number", "Customer name", "Mobile number", "Address",
                "Nationl number", "Deposite", "Collector name", "Representative name", "Area", "InvoiceDate",
                "MonthDate", "Items", "Plans"};

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
            sheet.Cell(excelRow, 8).SetValue(r.CollectorName);
            sheet.Cell(excelRow, 9).SetValue(r.RepresentativeName);
            sheet.Cell(excelRow, 10).SetValue(r.AreaName);
            sheet.Cell(excelRow, 11).SetValue(r.InvoiceDate);
            sheet.Cell(excelRow, 12).SetValue(r.MonthDate);
            sheet.Cell(excelRow, 13).SetValue(string.Join(", ", r.Items.Select(x => $"{x.Quantity} {x.ProductName}")));
            sheet.Cell(excelRow, 14).SetValue(r.TotalPrice);
            sheet.Cell(excelRow, 15).SetValue(string.Join(", ", r.Plans.Select(x => $"{x.Months} * {x.InstallmentAmount}")));
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