using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Core.Entities.CovenantModels;
using ErpSystemBeniSouef.Core.Entities.CustomerInvoices;
using ErpSystemBeniSouef.Infrastructer.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Infrastructer.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }

        public ApplicationDbContext()
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<MainArea> mainAreas { get; set; }
        public DbSet<SubArea> subAreas { get; set; }
        public DbSet<Company> company { get; set; }
        public DbSet<Representative> representatives { get; set; }
        public DbSet<Collector> collectors { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<Storekeeper> storekeepers { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<InvoiceItem> invoiceItems { get; set; }
        public DbSet<CashCstomerInvoice> cashCstomerInvoices { get; set; }
        public DbSet<CustomerInvoice> customerInvoices { get; set; }
        public DbSet<CustomerInvoiceItems> customerInvoiceItems { get; set; }
        public DbSet<InstallmentPlan> installmentPlans { get; set; }
        public DbSet<Commission> Commissions{ get; set; }
        public DbSet<PettyCash> pettyCashes{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //"Server=DESKTOP-NRGEJ6B\\SQLEXPRESS;Database=ErpSystemBeniSouef-DB;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true"
            //    );
            // optionsBuilder.UseSqlServer(
            var connectionString = "Server=DESKTOP-NRGEJ6B\\SQLEXPRESS;Database=ErpSystemBeniSouef-DB;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true";
            //);


            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            });

            //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ErpSystemBeniSouef-DB;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            #region Fix on DeleteCascade Behavior

            var cascadeFks = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFks)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            #endregion


            #region Edit Data Region


            //     builder.Entity<Product>()
            //     .HasOne(p => p.Company)
            //     .WithMany(c => c.Products)
            //     .HasForeignKey(p => p.CompanyId)
            //     .OnDelete(DeleteBehavior.SetNull); // 👈 بدل Cascade


            //     builder.Entity<Category>()
            //.HasOne(c => c.Company)
            //.WithMany()
            //.HasForeignKey(c => c.CompanyId)
            //.OnDelete(DeleteBehavior.Cascade); // تسيبها زي ما هي

            #endregion

        }
    }
}
