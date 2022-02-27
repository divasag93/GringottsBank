using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

#nullable disable

namespace GringottsBank.Plugins.Data.Sql
{
    public partial class GringottsBankContext : DbContext
    {
        public GringottsBankContext()
        {
        }

        public GringottsBankContext(DbContextOptions<GringottsBankContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Customer");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Gender).IsFixedLength(true);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Account");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Customer");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        public async Task SaveWithRetryAsync(int retryCounter = 0)
        {
            int maxRetry = 2;
            try
            {
                await SaveChangesAsync();
            }
            catch(System.Exception ex)
            {
                if (retryCounter == maxRetry)
                    throw new System.Exception("An error occured while saving");
                await SaveWithRetryAsync(++retryCounter);
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
