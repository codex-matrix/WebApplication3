using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionType> TransactionTypes => Set<TransactionType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<TransactionType>().ToTable("TransactionType");

            
            modelBuilder.Entity<Client>()
                        .Property(c => c.Balance)
                        .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                        .Property(t => t.Amount)
                        .HasColumnType("decimal(18,2)");

           
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType { TransactionTypeId = 1, Name = "Debit" },
                new TransactionType { TransactionTypeId = 2, Name = "Credit" }
            );
        }
    }
}
