using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using UglyToad.PdfPig;
using System.Text.RegularExpressions;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Infrastructer.Data.Context;

namespace ErpSystemBeniSouef.Infrastructer.Data
{
    public static class ExtrsctPdfFile
    {

        public static async Task ExtrsctPdfFilew()
        {

            List<string> lines = new();

            // 1) Extract text and lines
            using (var doc = PdfDocument.Open("C:/Users/Click/Desktop/ProductListPDF.pdf"))
            {
                foreach (var page in doc.GetPages())
                {
                    var text = page.Text;
                    // split on newlines; PDF extraction may fuse lines — you may need extra splitting heuristics.
                    var pageLines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    lines.AddRange(pageLines.Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)));
                }
            }



            #region MyRegion



            // 2) Define a regex to capture: [name..] [code] [price]
            // This is a heuristic — tune to your file. It assumes code is a sequence of digits (3-6) before the price.
            var pattern = new Regex(@"^(?<all>.+?)\s+(?<code>\d{2,6})\s+(?<price>[\d,]+(?:\.\d{1,2})?)\s*$", RegexOptions.Compiled);

            // 3) Parse lines into Product DTOs
            var parsedProducts = new List<Product>();
            foreach (var raw in lines)
            {
                var m = pattern.Match(raw);
                if (!m.Success)
                {
                    // حاول قاعدة بديلة: السعر في النهاية، بعدها الكود قبل السعر
                    var priceMatch = Regex.Match(raw, @"(?<price>[\d,]+(?:\.\d{1,2})?)\s*$");
                    if (priceMatch.Success)
                    {
                        var priceStr = priceMatch.Groups["price"].Value;
                        var beforePrice = raw.Substring(0, priceMatch.Index).Trim();
                        // آخر رقم في beforePrice نفترضه كـ code
                        var codeMatch = Regex.Match(beforePrice, @"(\d{2,6})\s*$");
                        if (codeMatch.Success)
                        {
                            var code = codeMatch.Value;
                            var name = beforePrice.Substring(0, codeMatch.Index).Trim();
                            decimal price2 = ParseDecimal(priceStr);
                            parsedProducts.Add(new Product
                            {
                                ProductName = name,
                                ProductCode = code,
                                SalePrice = price2,
                                PurchasePrice = 0m
                            });
                            continue;
                        }
                    }
                    // إن لم ينجح، سجل السطر للمعالجة اليدوية لاحقاً
                    // save to a list of "unparsed" for manual review
                    continue;
                }

                var namePart = m.Groups["all"].Value.Trim();
                var codePart = m.Groups["code"].Value.Trim();
                var pricePart = m.Groups["price"].Value.Trim();

                decimal price = ParseDecimal(pricePart);

                parsedProducts.Add(new Product
                {
                    ProductName = namePart,
                    ProductCode = codePart,
                    SalePrice = price,
                    PurchasePrice = 0m
                });
            }

            decimal ParseDecimal(string s)
            {
                // remove commas and any stray characters
                var cleaned = s.Replace(",", "").Trim();
                if (decimal.TryParse(cleaned, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var d))
                    return d;
                // حاول parse بالـ ar-EG إذا لزم
                if (decimal.TryParse(cleaned, NumberStyles.Number, new CultureInfo("ar-EG"), out d))
                    return d;
                return 0m;
            }

            // 4) Map categories (heuristic) and insert into DB

            foreach (var p in parsedProducts)
            {
                // مثال: نحاول استخراج اسم الـ category من بداية ProductName إذا كان مذكوراً
                // أو تعيين Category افتراضي.
                var categoryName = "Uncategorized";
                // مثال: إذا الاسم يبدأ بـ "اجهزة كهربائية" حاول استخراجه:
                if (p.ProductName.StartsWith("اجهزة كهربائية", StringComparison.OrdinalIgnoreCase))
                {
                    categoryName = "اجهزة كهربائية";
                    // وبعدها نزيل الجزء ده من الاسم لو أردت
                    p.ProductName = p.ProductName.Substring("اجهزة كهربائية".Length).Trim();
                }

                //var category = db.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
                //if (category == null)
                //{
                //    category = new Category { Name = categoryName };
                //    db.Categories.Add(category);
                //    await db.SaveChangesAsync();
                //}

                //p.CategoryId = category.Id;
                //// set company id if you have default
                //p.CompanyId = 1;

                //db.Products.Add(p);
            }



            #endregion


        }




    }
}
