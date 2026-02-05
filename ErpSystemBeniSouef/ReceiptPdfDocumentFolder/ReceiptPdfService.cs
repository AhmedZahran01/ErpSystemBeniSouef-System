using ErpSystemBeniSouef.Core.DTOs.Receipt;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.ReceiptPdfDocumentFolder
{
    public class ReceiptPdfService
    {

        public string OutputFolder { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReceiptsPDF");

        public ReceiptPdfService()
        {
            if (!Directory.Exists(OutputFolder))
                Directory.CreateDirectory(OutputFolder);
        }

        public List<string> GenerateReceipts(List<GetAllReceiptsDto> receipts)
        {
            var files = new List<string>();

            foreach (var receipt in receipts)
            {
                var doc = new ReceiptPdfDocument(receipt);

                var filePath = Path.Combine(
                    OutputFolder,
                    $"Receipt_{receipt.CustomerNumber}_{DateTime.Now:yyyyMMddHHmmss}.pdf"
                );

                doc.GeneratePdf(filePath);
                files.Add(filePath);
            }

            return files;
        }

        public string GenerateAllReceiptsInOnePdf(List<GetAllReceiptsDto> receipts)
        {
            var filePath = Path.Combine(
                OutputFolder,
                $"All_Receipts_{DateTime.Now:yyyyMMddHHmmss}.pdf"
            );

            var doc = new AllReceiptsPdfDocument(receipts);
            doc.GeneratePdf(filePath);

            return filePath;
        }


        public void OpenFolder()
        {
            Process.Start("explorer.exe", OutputFolder);
        }


    }
}
