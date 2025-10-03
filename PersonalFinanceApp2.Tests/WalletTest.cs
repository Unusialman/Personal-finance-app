using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
namespace PersonalFinanceApp2.Tests
{
    public class WalletTest
    {
        //правильность расчета текущего баланса
        [Fact]
        public void Wallet_CurrentBalance_CalculatesCorrectly()
        {
            
            var wallet = new Wallet
            {
                Id = 1,
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m,
                Transactions = new List<Transaction>()
            };

            // Добавляем транзакции
            wallet.Transactions.AddRange(new[]
            {
            new Transaction { Type = TransactionType.Income, Amount = 500m },
            new Transaction { Type = TransactionType.Expense, Amount = 200m },
            new Transaction { Type = TransactionType.Income, Amount = 300m },
            new Transaction { Type = TransactionType.Expense, Amount = 100m }
        });

            
            var currentBalance = wallet.CurrentBalance;

            
            Assert.Equal(1500m, currentBalance); // 1000 + (500+300) - (200+100) = 1500
        }

        //Возвращает начальный баланс, когда транзакций нет
        [Fact]
        public void Wallet_CurrentBalance_WithNoTransactions_ReturnsInitialBalance()
        {
            
            var wallet = new Wallet
            {
                Id = 1,
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m,
                Transactions = new List<Transaction>()
            };

            
            var currentBalance = wallet.CurrentBalance;

            
            Assert.Equal(1000m, currentBalance);
        }

        //Фильтрация по месяцу и правильная группировка по типам
        [Fact]
        public void Wallet_MonthlyIncomeExpense_CalculatesCorrectly()
        {
            
            var wallet = new Wallet
            {
                Id = 1,
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m,
                Transactions = new List<Transaction>()
            };

            var testDate = new DateTime(2024, 1, 15);
            wallet.Transactions.AddRange(new[]
            {
            new Transaction { Type = TransactionType.Income, Amount = 500m, Date = testDate },
            new Transaction { Type = TransactionType.Expense, Amount = 200m, Date = testDate },
            new Transaction { Type = TransactionType.Income, Amount = 300m, Date = new DateTime(2024, 2, 15) }, // Другой месяц
            new Transaction { Type = TransactionType.Expense, Amount = 100m, Date = testDate }
        });

            
            var (income, expense) = wallet.MonthlyIncomeExpense(2024, 1);

            
            Assert.Equal(500m, income);
            Assert.Equal(300m, expense); // 200 + 100
        }

        //возвращает 0 при отсутствии транзакций
        [Fact]
        public void Wallet_MonthlyIncomeExpense_NoTransactions_ReturnsZero()
        {
            
            var wallet = new Wallet
            {
                Id = 1,
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m,
                Transactions = new List<Transaction>()
            };

            
            var (income, expense) = wallet.MonthlyIncomeExpense(2024, 1);

            
            Assert.Equal(0m, income);
            Assert.Equal(0m, expense);
        }
    }
    
}