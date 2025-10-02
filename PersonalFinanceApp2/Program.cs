using System.Collections.Generic;

namespace PersonalFinanceApp2
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
    class Program
    {
        private static void Main(string[] args)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                try
                {

                }
                catch (ArgumentException)
                {
                    Console.WriteLine("начальное значение баланса не может быть меньше 0");
                }
                // получаем объекты из бд и выводим на консоль
                List<Wallet> Wallets = db.Wallets.ToList();

                List<Transaction> transactions = db.Transactions.ToList();
                Console.WriteLine("Список объектов:");
                List<Transaction> TransactionInWallet; 
                
                foreach (Wallet w in Wallets)
                {
                    w.TransactionsPerMonth(2025, 5);
                }

            }
        }
    }
}