namespace ErpSystemBeniSouef.Core.DTOs.Receipt;

public class GetAllReceiptsDto
{ 
    public int DisplayUIId { get; set; }
    public int MonthlyInstallmentId { get; set; }
    public int CustomerNumber { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string Address { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public string RepresentativeName { get; set; } = string.Empty;
    public string NationlNumber { get; set; } = string.Empty;
    public decimal Deposit { get; set; }
    public string Plans { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Items { get; set; } = string.Empty;
    public string CollectorName { get; set; } = string.Empty;
    public DateTime FirstInvoiceDate { get; set; }
    public bool IsPaid { get; set; }
    public decimal InstallmentAmount { get; set; }
    public DateTime InstallmentDueDate { get; set; }
    public string InstallmentAmountText { get; set; } = string.Empty;


    // تحوّل Items إلى قائمة أصناف مفصّلة بدل string
    public List<ReceiptItemDto> ItemsList { get; set; } = new List<ReceiptItemDto>();
    public List<ReceiptDetailDto> Receipts { get; set; } = new List<ReceiptDetailDto>();
}

public class ReceiptDetailDto
{      
    public int MonthlyInstallmentId { get; set; }
    public decimal InstallmentAmount { get; set; }
    public DateTime InstallmentDueDate { get; set; }
    public bool IsPaid { get; set; }
    public string CollectorName { get; set; } = string.Empty;
}

  
    public class ReceiptItemDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public string MeasurementUnit { get; set; } = string.Empty;
        public decimal Total => UnitPrice * Quantity; 

    }
 
 
