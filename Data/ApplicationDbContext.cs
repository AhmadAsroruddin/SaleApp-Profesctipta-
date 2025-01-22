using Microsoft.EntityFrameworkCore;
using SaleApp.Models;

namespace SaleApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .ToTable("SO_ITEM", "dbo");
            modelBuilder.Entity<Item>()
                .ToTable("SO_ORDER", "dbo");
            modelBuilder.Entity<Customer>()
                .ToTable("COM_CUSTOMER", "dbo");
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.COM_CUSTOMER_ID)
                .HasColumnName("COM_CUSTOMER_ID")  // Nama kolom
                .ValueGeneratedOnAdd(); 

        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

          

            return base.SaveChanges();
        }
    }
}
