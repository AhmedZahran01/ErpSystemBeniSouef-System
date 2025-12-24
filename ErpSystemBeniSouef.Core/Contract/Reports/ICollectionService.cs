using ErpSystemBeniSouef.Core.DTOs.Reports;
using ErpSystemBeniSouef.Core.DTOs.Reports.MonthlyCollectingDtos;

namespace ErpSystemBeniSouef.Core.Contract.Reports
{
    public interface ICollectionService
    {
        // 1️⃣ Monthly Installments
        Task<List<MonthlyCollectionItemDto>> GetMonthlyInstallmentsAsync(
            int collectorId,
            DateTime month
        );

        // 2️⃣ Installment Sales Report
        Task<List<InstallmentReportDto>> GetInstallmentSalesReportAsync(
            DateTime fromDate,
            DateTime toDate,
            int collectorId
        );

        // 3️⃣ Representative Covenants
        //Task<List<CovenantReportRowDto>> GetRepresentativeCovenantsAsync(
        //    DateTime fromDate,
        //    DateTime toDate,
        //    int collectorId
        //);

        // 4️⃣ Representative Cash Invoices
        //Task<List<CashInvoicesReportDto>> GetRepresentativeCashInvoicesAsync(
        //    DateTime fromDate,
        //    DateTime toDate,
        //    int collectorId
        //);

        // 5️⃣ Installment Sales Commission Report
        Task<List<RepresentativeCommissionReportDto>> GetAllItemsInstallmentSalesReportAsync(
            DateTime fromDate,
            DateTime toDate,
            int collectorId
        );

        // 6️⃣ Print Customers Account
        Task<(Byte[] FileContent, decimal totalDeposits)> PrintCustomersAccountAsync(
            DateTime fromDate,
            DateTime toDate,
            int representativeId
        );


        Task<(List<CashInvoicesReportDto>, Byte[] FileContent, decimal totalCash)> GetRepresentativeCashInvoicesAsync(
           DateTime fromDate, DateTime toDate, int collectorId);

        Task<(List<CovenantReportRowDto>, Byte[] FileContent, decimal totalCommision)> GetRepresentativeCovenantsAsync(
          DateTime fromDate,
          DateTime toDate,
          int collectorId
        );


    }
}
