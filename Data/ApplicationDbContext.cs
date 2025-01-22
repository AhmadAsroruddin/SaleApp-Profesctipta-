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
            modelBuilder.Entity<Order>()
                .ToTable("SO_ORDER", "dbo");
            modelBuilder.Entity<Customer>()
                .ToTable("COM_CUSTOMER", "dbo");
            
            modelBuilder.Entity<Customer>()
                .Property(c => c.COM_CUSTOMER_ID)
                .HasColumnName("COM_CUSTOMER_ID")  // Nama kolom
                .ValueGeneratedOnAdd(); 
            modelBuilder.Entity<Item>()
                .Property(c => c.SO_ITEM_ID)
                .HasColumnName("SO_ITEM_ID")  // Nama kolom
                .ValueGeneratedOnAdd(); 
            modelBuilder.Entity<Order>()
                .Property(c => c.SO_ORDER_ID)
                .HasColumnName("SO_ORDER_ID")  // Nama kolom
                .ValueGeneratedOnAdd(); 

                modelBuilder.Entity<Item>()
                .Property(e => e.PRICE)
                .HasColumnType("FLOAT"); 

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
