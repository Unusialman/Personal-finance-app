using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp2
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() 
        {
            try
            {
                var databaseCreator = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
                if (databaseCreator != null)
                    databaseCreator.CreateTables();
            }
            catch (Microsoft.Data.Sqlite.SqliteException)
            {
                //игнор "ошибки" если файл бд уже создан
            }
        }   
        public DbSet<Wallet> Wallets => Set<Wallet>() ;
        public DbSet<Transaction> Transactions => Set<Transaction>();
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PersonalFinance.db");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //1 кошелек и много транзакций
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Transactions)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.Id_Wallet)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wallet>()
                .Property(w => w.InitialBalance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Type)
                .HasConversion<string>();
        }
    }
}
