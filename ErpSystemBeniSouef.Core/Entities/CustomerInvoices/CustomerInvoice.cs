namespace ErpSystemBeniSouef.Core.Entities.CustomerInvoices;

public class CustomerInvoice:BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<CustomerInvoiceItems>? Items { get; set; }
    public ICollection<InstallmentPlan>? Installments { get; set; }
}