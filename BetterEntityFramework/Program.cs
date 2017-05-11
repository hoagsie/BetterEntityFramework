using System.Linq;
using BetterEntityFramework.Extensions;
using BetterEntityFramework.StoreData;
using Microsoft.EntityFrameworkCore;
using Data = BetterEntityFramework.DataService;
using IsolationLevel = System.Data.IsolationLevel;

namespace BetterEntityFramework
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var data = new Data();

            var newConfig = data.Configure().Builder.UseInMemoryDatabase();
            data.UseConfiguration(newConfig);

            // or

            var options = new DataOptionsBuilder();
            options.Builder.UseInMemoryDatabase();

            data = new DataService(options);

            data.Service.Category.UpdateWhere(category => category.Publish == false, category => new Category { Publish = true }).Wait();
            data.Service.Product.DeleteWhere(product => product.Publish == false).Wait();

            var restrictiveTransaction = new DataOptionsBuilder().Builder.UseSqlServer("connection string", builder => builder.ExecutionStrategy(context =>
            {
                context.Context.Database.BeginTransaction(IsolationLevel.Serializable);
                return context.Context.Database.CreateExecutionStrategy();
            }));

            using (var isolatedOperation = data.WithScopedService( restrictiveTransaction))
            {
                isolatedOperation.Service.User.DeleteWhere(user => user.Inactive).Wait();
            }

            data.Service.BulkInsert(data.Service.Category.Where(category => category.Publish), data.Service.CategorySubscription).Wait();
            data.Service.ClearCache();
        }
    }
}
