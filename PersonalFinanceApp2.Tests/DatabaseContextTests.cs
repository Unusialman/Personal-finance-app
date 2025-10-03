using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp2.Tests
{
    public class DatabaseContextTests
    {
        //Проверка, что контекст БД создается без ошибок
        [Fact]
        public void DatabaseContext_CanBeCreated()
        {
            // Arrange & Act

            using var context = new DataBaseContext();

            // Assert
            Assert.NotNull(context);
        }

        //Проверка доступности DbSet для кошельков
        [Fact]
        public void DatabaseContext_WalletsDbSet_IsAvailable()
        {
            // Arrange

            using var context = new DataBaseContext();

            // Act & Assert
            Assert.NotNull(context.Wallets);
        }

        //Проверка доступности DbSet для транзакций
        [Fact]
        public void DatabaseContext_TransactionsDbSet_IsAvailable()
        {
            // Arrange

            using var context = new DataBaseContext();

            // Act & Assert
            Assert.NotNull(context.Transactions);
        }
    }
}
