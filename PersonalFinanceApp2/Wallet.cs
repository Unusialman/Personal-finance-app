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
        // функция вычисляющая месячный доход/расход 
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