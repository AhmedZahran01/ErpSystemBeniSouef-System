using ErpSystemBeniSouef.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            "Server=DESKTOP-NRGEJ6B\\SQLEXPRESS;Database=ErpSystemBeniSouef-DB;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True;MultipleActiveResultSets=true"
                );
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


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

