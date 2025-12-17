namespace ErpSystemBeniSouef.Core.Contract.CustomerInvoice;

public interface ICustomerInvoiceService
{
    Task<ServiceResponse<bool>> CreateCustomerInvoiceAsync(CreateCustomerInvoiceDTO dto);
    Task<ServiceResponse<IReadOnlyList<ReturnCustomerInvoiceListDTO>>> GetAllCustomerInvoicesAsync();
    Task<ServiceResponse<ReturnCustomerInvoiceDetailsDTO>> GetCustomerInvoiceByIdAsync(int invoiceId);
    Task<ServiceResponse<bool>> DeleteCustomerAsync(int customerId);
    Task<ServiceResponse<bool>> UpdateCustomerInvoiceAsync(int invoiceId, UpdateCustomerInvoiceDTO dto);
    Task<ServiceResponse<List<MonthlyInstallmentDto>>> GetMonthlyInstallmentsByInvoiceIdAsync(int customerInvoiceId);
    Task<ServiceResponse<bool>> UpdateInstallmentsAsync(UpdateMonthlyInstallmentsDto dto);
    Task<ServiceResponse<bool>> InstallmentsTransferAsync(int customerInvoiceId);
}