using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Infrastructer.Data.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Infrastructer.Data
{
    public static class StoreDokContextSeed
    {
        #region Seeding Function Region

        public async static Task SeedAsync(ApplicationDbContext dbcontext)
        {
            #region Company Region 

            if (!dbcontext.company.Any()) // Check if the database is empty
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "DataSeeding", "CompanyData.json");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"ملف CompanyData.json مش موجود في: {filePath}");

                var subRegionData = File.ReadAllText(filePath);

                var subRegions = JsonSerializer.Deserialize<List<Company>>(subRegionData);

                if (subRegions?.Count() > 0)
                {
                    foreach (var subRegion in subRegions)
                    {
                        dbcontext.Set<Company>().Add(subRegion);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            #endregion

            #region category Region 

            if (!dbcontext.categories.Any()) // Check if the database is empty
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "DataSeeding", "Category.json");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"ملف CompanyData.json مش موجود في: {filePath}");

                var subRegionData = File.ReadAllText(filePath);

                var subRegions = JsonSerializer.Deserialize<List<Category>>(subRegionData);

                if (subRegions?.Count() > 0)
                {
                    foreach (var subRegion in subRegions)
                    {
                        dbcontext.Set<Category>().Add(subRegion);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            #endregion


            #region Main Areas Region

            if (!dbcontext.mainAreas.Any()) // Check if the database is empty
            {
                //var mainRegionsData = File.ReadAllText
                //    ("C:\\Users\\Click\\Desktop\\الشريف سيستم\\Local Copy Of Project\\$ErpSystemBeniSouef\\ErpSystemBeniSouef.Infrastructer\\Data\\DataSeeding\\mainRegions.json");

                var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "DataSeeding", "mainRegions.json");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"ملف subRegion.json مش موجود في: {filePath}");

                var mainRegionsData = File.ReadAllText(filePath);

                var mainRegions = JsonSerializer.Deserialize<List<MainArea>>(mainRegionsData);

                if (mainRegions?.Count() > 0)
                {
                    foreach (var mainArea in mainRegions)
                    {
                        dbcontext.Set<MainArea>().Add(mainArea);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }

            #endregion

            #region Sub Areas Region 

            if (dbcontext.subAreas.Any()) // Check if the database is empty
            //if (!dbcontext.subAreas.Any()) // Check if the database is empty
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "DataSeeding", "subRegion.json");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"ملف subRegion.json مش موجود في: {filePath}");

                var subRegionData = File.ReadAllText(filePath);

                var subRegions = JsonSerializer.Deserialize<List<SubArea>>(subRegionData);

                if (subRegions?.Count() > 0)
                {
                    foreach (var subRegion in subRegions)
                    {
                        dbcontext.Set<SubArea>().Add(subRegion);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            #endregion

            #region Products Region 

            if (!dbcontext.products.Any()) // Check if the database is empty
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "DataSeeding", "Products.json");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"ملف dummy_products.json مش موجود في: {filePath}");

                var subRegionData = File.ReadAllText(filePath);

                var subRegions = JsonSerializer.Deserialize<List<Product>>(subRegionData);

                if (subRegions?.Count() > 0)
                {
                    foreach (var subRegion in subRegions)
                    {
                        dbcontext.Set<Product>().Add(subRegion);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            #endregion


        }
        #endregion


    }
}
