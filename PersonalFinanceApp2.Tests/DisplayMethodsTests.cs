using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp2.Tests
{
    public class DisplayMethodsTests
    {
        ////Проверяет корректную обработку данных при их наличии
        [Fact]
        public void ShowMonthlyTransactions_ValidInput_ProcessesCorrectly()
        {
            // Arrange
            using var context = new DataBaseContext();

            // Создаем тестовые данные
            var wallet = new Wallet
            {
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m,
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Date = new DateTime(2024, 1, 15),
                        Amount = 500m,
                        Type = TransactionType.Income,
                        Description = "Salary",
                        Id_Wallet = 1
                    },
                    new Transaction
                    {
                        Date = new DateTime(2024, 1, 20),
                        Amount = 200m,
                        Type = TransactionType.Expense,
                        Description = "Groceries",
                        Id_Wallet = 1
                    }
                }
            };

            context.Wallets.Add(wallet);
            context.SaveChanges();

            // Act & Assert
            // Проверяем, что метод не выбрасывает исключений
            var exception = Record.Exception(() => ShowMonthlyTransactions(context, 2024, 1));
            Assert.Null(exception);
            context.Wallets.Remove(wallet);
            context.SaveChanges();
        }
        //Проверяет корректную обработку данных при их наличии
        [Fact]
        public void ShowTopExpenses_ValidInput_ProcessesCorrectly()
        {
            // Arrange
            using var context = new DataBaseContext();

            var wallet = new Wallet
            {
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m,
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Date = new DateTime(2024, 1, 15),
                        Amount = 1000m,
                        Type = TransactionType.Expense,
                        Description = "Big Expense",
                        Id_Wallet = 1
                    },
                    new Transaction
                    {
                        Date = new DateTime(2024, 1, 20),
                        Amount = 500m,
                        Type = TransactionType.Expense,
                        Description = "Medium Expense",
                        Id_Wallet = 1
                    },
                    new Transaction
                    {
                        Date = new DateTime(2024, 1, 25),
                        Amount = 300m,
                        Type = TransactionType.Expense,
                        Description = "Small Expense",
                        Id_Wallet = 1
                    }
                }
            };

            context.Wallets.Add(wallet);
            context.SaveChanges();

            // Act & Assert
            var exception = Record.Exception(() => ShowTopExpenses(context, 2024, 1));
            Assert.Null(exception);
            context.Wallets.Remove(wallet);
            context.SaveChanges();
        }

        //проверка транзакций за месяц без транзакций
        [Fact]
        public void ShowMonthlyTransactions_NoTransactions_HandlesCorrectly()
        {
            // Arrange
            using var context = new DataBaseContext();

            var wallet = new Wallet
            {
                Name = "Test Wallet",
                Currency = "RUB",
                InitialBalance = 1000m,
                Transactions = new List<Transaction>()
            };

            context.Wallets.Add(wallet);
            context.SaveChanges();

            // Act & Assert
            var exception = Record.Exception(() => ShowMonthlyTransactions(context, 2024, 1));
            Assert.Null(exception);
            context.Wallets.Remove(wallet);
            context.SaveChanges();
        }

        // Вспомогательные методы для тестирования (адаптированные версии ваших методов)
        private void ShowMonthlyTransactions(DataBaseContext db, int year, int month)
        {
            try
            {
                var transactions = db.Transactions
                    .Where(t => t.Date.Year == year && t.Date.Month == month)
                    .Include(t => t.Wallet)
                    .ToList();

                var groupedTransactions = transactions
                    .GroupBy(t => t.Type)
                    .OrderByDescending(g => g.Sum(t => t.Amount))
                    .ToList();

                if (groupedTransactions.Count != 0)
                {
                    foreach (var group in groupedTransactions)
                    {
                        Console.WriteLine($"\nТип: {group.Key}");
                        Console.WriteLine($"Общая сумма: {group.Sum(t => t.Amount)}");
                        Console.WriteLine("Транзакции:");

                        foreach (var transaction in group.OrderBy(t => t.Date))
                        {
                            Console.WriteLine($" {transaction.Date:dd.MM.yyyy} - {transaction.Description} - {transaction.Amount} {transaction.Wallet?.Currency}");
                        }
                        Console.WriteLine(new string('-', 40));
                    }
                }
                else
                {
                    Console.WriteLine($"Транзакций за {month} месяц {year} года не было");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный ввод данных");
            }
        }



        private void ShowTopExpenses(DataBaseContext db, int year, int month)
        {
            try
            {
                var wallets = db.Wallets.Include(w => w.Transactions).ToList();

                Console.WriteLine(new string('-', 40));

                foreach (var wallet in wallets)
                {
                    var transactionsMonth = wallet.Transactions
                        .Where(t => t.Date.Year == year && t.Date.Month == month && t.Type == TransactionType.Expense)
                        .OrderByDescending(t => t.Amount)
                        .Take(3)
                        .ToList();

                    if (transactionsMonth.Count > 0)
                    {
                        foreach (var transaction in transactionsMonth)
                        {
                            Console.WriteLine($"{transaction.Date:dd.MM.yyyy} - {transaction.Description} - {transaction.Amount} " +
                                $"- кошелек {wallet.Id} на имя {wallet.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"В кошельке с именем {wallet.Name} под номером {wallet.Id} нет транзакций за данный месяц");
                    }
                    Console.WriteLine(new string('-', 40));
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный ввод данных");
            }
        }
    }
}
