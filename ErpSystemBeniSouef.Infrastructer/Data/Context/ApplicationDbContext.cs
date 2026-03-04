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
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<PettyCash> pettyCashes { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<RepresentativeWithdrawal> representativeWithdrawals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //"Server=DESKTOP-NRGEJ6B\\SQLEXPRESS;Database=ErpSystemBeniSouef-DB;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true"
            //    );
            // optionsBuilder.UseSqlServer(
            var connectionString = "Server=DESKTOP-NRGEJ6B\\SQLEXPRESS;Database=ErpSystemBeniSouef-DB;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true";
           // var connectionString = "Server=DESKTOP-V116C4B\\SQLEXPRESS;Database=ErpSystemBeniSouef-DB;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true";
            //);


            //optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            //{
            //    sqlOptions.EnableRetryOnFailure(
            //        maxRetryCount: 5,
            //        maxRetryDelay: TimeSpan.FromSeconds(10),
            //        errorNumbersToAdd: null
            //    );
            //});

                         optionsBuilder.UseSqlServer(
                 connectionString,
                 x => x.EnableRetryOnFailure(0)
             );


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

            // Enable Cascade Delete for Customer -> CustomerInvoice
            builder.Entity<CustomerInvoice>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enable Cascade Delete for CustomerInvoice -> CustomerInvoiceItems
            builder.Entity<CustomerInvoiceItems>()
                .HasOne(i => i.Invoice)
                .WithMany(inv => inv.Items)
                .HasForeignKey(i => i.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enable Cascade Delete for CustomerInvoice -> InstallmentPlan
            builder.Entity<InstallmentPlan>()
                .HasOne(p => p.Invoice)
                .WithMany(inv => inv.Installments)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enable Cascade Delete for CustomerInvoice -> MonthlyInstallment
            builder.Entity<MonthlyInstallment>()
                .HasOne(m => m.Invoice)
                .WithMany()
                .HasForeignKey(m => m.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enable Cascade Delete for CustomerInvoiceItems -> Commission
            builder.Entity<Commission>()
                .HasOne(c => c.InvoiceItem)
                .WithMany()
                .HasForeignKey(c => c.InvoiceItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // another database Cascade here to prevent SQL Server Multiple Cascade Path errors.
            builder.Entity<Commission>()
                .HasOne(c => c.Invoice)
                .WithMany()
                .HasForeignKey(c => c.InvoiceId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Enable Cascade Delete for CustomerInvoice -> Discount
            builder.Entity<Discount>()
                .HasOne(d => d.Invoice)
                .WithMany()
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

        }
    }
}
