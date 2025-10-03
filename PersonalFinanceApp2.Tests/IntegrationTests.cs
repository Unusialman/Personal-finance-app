using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp2.Tests
{
    public class IntegrationTests 
    {
        private readonly DataBaseContext _context;

        public IntegrationTests()
        {

            _context = new DataBaseContext();
            _context.Database.EnsureCreated();
        }

        //возможность добавления новыхкошельков в бд
        [Fact]
        public async Task Can_Add_Wallet_To_Database()
        {
            // Arrange
            var wallet = new Wallet
            {
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m
            };

            // Act
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            // Assert
            var savedWallet = await _context.Wallets.FindAsync(wallet.Id);
            Assert.NotNull(savedWallet);
            Assert.Equal("Test Wallet", savedWallet.Name);
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }

        //возможность добавления транзакций в бд
        [Fact]
        public async Task Can_Add_Transaction_To_Database()
        {
            // Arrange
            var wallet = new Wallet
            {
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m
            };

            var transaction = new Transaction
            {
                Date = DateTime.Now,
                Amount = 100m,
                Type = TransactionType.Expense,
                Description = "Test Expense",
                Wallet = wallet
            };

            // Act
            _context.Wallets.Add(wallet);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Assert
            var savedTransaction = await _context.Transactions.FindAsync(transaction.Id);
            Assert.NotNull(savedTransaction);
            Assert.Equal("Test Expense", savedTransaction.Description);
            Assert.Equal(wallet.Id, savedTransaction.Id_Wallet);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }


        //проверка связи один-ко-многим
        [Fact]
        public async Task Wallet_Transactions_Relationship_Works()
        {
            // Arrange
            var wallet = new Wallet
            {
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m
            };

            var transaction1 = new Transaction
            {
                Date = DateTime.Now,
                Amount = 100m,
                Type = TransactionType.Expense,
                Description = "Expense 1",
                Wallet = wallet
            };

            var transaction2 = new Transaction
            {
                Date = DateTime.Now,
                Amount = 200m,
                Type = TransactionType.Income,
                Description = "Income 1",
                Wallet = wallet
            };

            // Act
            _context.Wallets.Add(wallet);
            _context.Transactions.AddRange(transaction1, transaction2);
            await _context.SaveChangesAsync();

            // Assert
            var savedWallet = await _context.Wallets
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.Id == wallet.Id);

            Assert.NotNull(savedWallet);
            Assert.Equal(2, savedWallet.Transactions.Count);
            _context.Wallets.Remove(wallet);
            _context.Transactions.RemoveRange(transaction1, transaction2);
            await _context.SaveChangesAsync();
        }


    }
}
