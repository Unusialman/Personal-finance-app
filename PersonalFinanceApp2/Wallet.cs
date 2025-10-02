using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp2
{
    // Класс кошелька с атрибутами для БД

    public class Wallet
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Currency { get; set; }


        public decimal InitialBalance { get; set; }


        public List<Transaction> Transactions;
        
        public decimal CurrentBalance
        {
            get
            {
                decimal incomeTotal = Transactions
                    .Where(t => t.Type == TransactionType.Income)
                    .Sum(t => t.Amount);

                decimal expenseTotal = Transactions
                    .Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);

                return InitialBalance + incomeTotal - expenseTotal;
            }
        }
        public void TransactionsPerMonth(int Year, int Month)
        {
            try
            {
                var _Transactions = Transactions.Where(t => t.Date.Year == Year && t.Date.Month == Month)
                    .GroupBy(t => t.Type).OrderByDescending(g => g.Sum(t => t.Amount)).ToList();
                if (_Transactions.Count !=0)
                {
                    foreach (var group in _Transactions)
                    {
                        Console.WriteLine($"\nТип: {group.Key}");
                        Console.WriteLine($"Общая сумма: {group.Sum(t => t.Amount)} {Currency}");
                        Console.WriteLine("Транзакции:");

                        foreach (var transaction in group.OrderBy(t => t.Date))
                        {
                            Console.WriteLine($"  {transaction.Date:dd.MM.yyyy} - {transaction.Description} - {transaction.Amount} {Currency}");
                        }
                        Console.WriteLine(new string('-', 40));
                    }
                }
                else
                    Console.WriteLine($"в кошельке с именем {Name} транзакций за {Month} месяц {Year} года не было");
            }
            catch(ArgumentNullException) 
            {
                Console.WriteLine($"в кошельке {Name} транзакций не было");
            }

        }

        public (decimal, decimal) MonthlyIncomeExpense(int Year, int Month)
        {
            decimal incomeTotal = Transactions
                .Where(t => t.Type == TransactionType.Income && t.Date.Year == Year && t.Date.Month == Month)
                .Sum(t => t.Amount);

            decimal expenseTotal = Transactions
                .Where(t => t.Type == TransactionType.Expense && t.Date.Year == Year && t.Date.Month == Month)
                .Sum(t => t.Amount);

            return (incomeTotal, expenseTotal);
        }
    }
}