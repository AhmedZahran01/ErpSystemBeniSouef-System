namespace ErpSystemBeniSouef.Core.Contract.Reports
{
    public interface ICollectorsReports
    {
        // المبيعات تقسيط 
        Task<(List<InstallmentReportDto>, decimal totalDeposits, Byte[] fileContent)> GetInstallmentSalesReportAsync(
            DateTime fromDate, DateTime toDate, int collectorId);

        // نسبة المندبة الجديدة 
        Task<(List<RepresentativeCommissionReportDto>, decimal totalCommissionPercentage, Byte[] fileContent)> GetAllItemsInstallmentSalesReportAsync(
            DateTime fromDate, DateTime toDate, int collectorId);

        // المبيعات كاش
        Task<(List<CashInvoicesReportDto>, Byte[] fileContent, decimal totalCash)> GetRepresentativeCashInvoicesAsync(
            DateTime fromDate, DateTime toDate, int collectorId);

        // نسبة المرتجعات
        Task<(List<CovenantReportRowDto>, Byte[] fileContent, decimal totalCommision)> GetRepresentativeCovenantsAsync(
            DateTime fromDate, DateTime toDate, int collectorId);

        Task<RepresentativeReportSummaryDto> GetRepresentativeSummaryAsync(
            DateTime fromDate, DateTime toDate, int representativeId);
    }
}