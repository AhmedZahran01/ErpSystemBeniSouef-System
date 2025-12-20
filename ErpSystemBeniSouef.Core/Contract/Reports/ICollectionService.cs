namespace ErpSystemBeniSouef.Core.Contract.Reports;

public interface ICollectionService
{

    // 2️⃣ Installment Sales Report
    Task<List<InstallmentReportDto>> GetInstallmentSalesReportAsync(
    DateTime fromDate,
    DateTime toDate,
    int collectorId);

    //3️⃣ Representative Covenants
    Task<(List<CovenantReportRowDto>, Byte[] FileContent, decimal totalCommision)> GetRepresentativeCovenantsAsync(
    DateTime fromDate,
    DateTime toDate,
    int collectorId);

    // 4️⃣ Representative Cash Invoices
    Task<(List<CashInvoicesReportDto>, Byte[] FileContent, decimal totalCash)> GetRepresentativeCashInvoicesAsync(
    DateTime fromDate,
    DateTime toDate,
    int collectorId);

    // 5️⃣ Installment Sales Commission Report
    Task<List<RepresentativeCommissionReportDto>> GetAllItemsInstallmentSalesReportAsync(
    DateTime fromDate,
    DateTime toDate,
    int collectorId);

    // 6️⃣ Print Customers Account
    Task<(Byte[] FileContent, decimal totalDeposits)> PrintCustomersAccountAsync(
        DateTime fromDate,
        DateTime toDate,
        int representativeId
    );
}