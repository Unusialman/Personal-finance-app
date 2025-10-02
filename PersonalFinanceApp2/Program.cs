using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Quic;
using System.Xml.Linq;

namespace PersonalFinanceApp2
{
    class Program
    {
        private static void Main(string[] args)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                int input;

                do
                {
                    Console.WriteLine(@"
выберите номер:
1) вывести все транзакции за месяц.
2) вывести 3 самые большие траты за указанный месяц для каждого кошелька
3) выход");
                    try
                    {
                        input = Convert.ToInt32(Console.ReadLine());
                        switch (input)
                        {
                            case 1:
                                {
                                    ShowMonthlyTransactions(db);
                                    break;
                                }
                            case 2:
                                {
                                    ShowTopExpenses(db);
                                    break;

                                }
                            case 3:
                                {
                                    Console.WriteLine("прощайте, пользователь");
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("введите номер одного из предложенных вариантов");
                                    break;
                                }

                        }
                    }
                    catch (FormatException)
                    {
                        input = 0;
                        Console.WriteLine("пожалуйста введите номер действия");
                    }

                } while (input != 3);

            }
        }
        private static void ShowMonthlyTransactions(DataBaseContext db)
        {
            try
            {

                Console.WriteLine("\nВведите год: ");
                int Year = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\nВведите месяц: ");
                int Month = Convert.ToInt32(Console.ReadLine());

                List<Transaction> Transactions = db.Transactions.ToList();
                List<Wallet> wallets = db.Wallets.ToList();

                var transactions = Transactions.Where(t => t.Date.Year == Year && t.Date.Month == Month)
                    .GroupBy(t => t.Type).OrderByDescending(g => g.Sum(t => t.Amount)).ToList();

                
                if (transactions.Count != 0)
                {
                    foreach (var group in transactions)
                    {
                        Console.WriteLine($"\nТип: {group.Key}");
                        Console.WriteLine($"Общая сумма: {group.Sum(t => t.Amount)}");
                        Console.WriteLine("Транзакции:");

                        foreach (var transaction in group.OrderBy(t => t.Date))
                        {
                            Console.WriteLine($"  {transaction.Date:dd.MM.yyyy} - {transaction.Description} - {transaction.Amount} " +
                                $"- кошелек {wallets.Where(w => w.Id == transaction.Id_Wallet).Single().Id} на имя {wallets.Where(w => w.Id == transaction.Id_Wallet).Single().Name}");
                        }
                        Console.WriteLine(new string('-', 40));
                    }

                }
                else Console.WriteLine("транзакций за указаный месяц нет");

                
            }
            catch (FormatException)
            {
                Console.WriteLine("неврный ввод данных");
                
            }
        }
        private static void ShowTopExpenses(DataBaseContext db)
        {
            try
            {

                Console.WriteLine("\nВведите год: ");
                int Year = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\nВведите месяц: ");
                int Month = Convert.ToInt32(Console.ReadLine());

                List<Wallet> wallets = db.Wallets.ToList();
                List<Transaction> transactionsMounth;


                Console.WriteLine(new string('-', 40));


                foreach (Wallet wallet in wallets)
                {
                    transactionsMounth = wallet.Transactions.Where(t => t.Date.Year == Year && t.Date.Month == Month && t.Type == TransactionType.Expense).OrderByDescending(t => t.Amount).Take(3).ToList();
                    if (transactionsMounth.Count > 0)
                    {
                        foreach (Transaction transaction in transactionsMounth)
                        {
                            Console.WriteLine($"\n  {transaction.Date:dd.MM.yyyy} - {transaction.Description} - {transaction.Amount} " +
                                $"- кошелек {wallets.Where(w => w.Id == transaction.Id_Wallet).Single().Id} на имя {wallets.Where(w => w.Id == transaction.Id_Wallet).Single().Name}");

                        }

                    }
                    else
                    {
                        Console.WriteLine($"В кошельке с имеем {wallet.Name} под номером {wallet.Id} нет транзакций за данный месяц");
                    }
                    Console.WriteLine(new string('-', 40));
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("неврный ввод данных");
                
            }
        }
    }
}