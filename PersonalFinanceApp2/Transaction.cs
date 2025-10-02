using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp2
{
    public enum TransactionType
    {
        Income, 
        Expense
        
          
    }


    public class Transaction
    {

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public string Description { get; set; }

        public int Id_Wallet { get; set; }

        public Wallet Wallet { get; set; }

    }

}
