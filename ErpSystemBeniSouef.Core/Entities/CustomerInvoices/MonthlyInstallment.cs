namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices;

public class MonthlyInstallment : BaseEntity
{
    public int InvoiceId { get; set; }
    public CustomerInvoice Invoice { get; set; } = default!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    public int CollectorId { get; set; }
    public Collector Collector { get; set; } = default!;

    public DateTime MonthDate { get; set; } // لكل شهر
    public decimal Amount { get; set; }     // قيمة القسط
    public decimal CollectedAmount { get; set; } = 0;
   // public bool IsCollected => CollectedAmount >= Amount;
    public bool IsPaid { get; set; } = false;
    public bool IsDelayed { get; set; } // هل اتأجل للشهر اللي بعده؟
}