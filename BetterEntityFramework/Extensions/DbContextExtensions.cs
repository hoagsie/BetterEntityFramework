using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using BetterEntityFramework.DataReaders;
using BetterEntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BetterEntityFramework.Extensions
{
    internal static class DbContextExtensions
    {
        public static void ClearCache<TEntity>(this DbContext context, IEnumerable<TEntity> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var entries = context.ChangeTracker.Entries().Where(entry => selector.Any(entity => entry.Entity.Equals(entity)));

            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }

        public static void ClearCache(this DbContext context)
        {
            ClearCache(context, context.ChangeTracker.Entries().Select(entry => entry.Entity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="context"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="batchSize"></param>
        /// <param name="timeout"></param>
        /// <param name="notifyFrequency"></param>
        /// <param name="observer"></param>
        /// <returns></returns>
        public static async Task BulkInsert<TSource>(this DbContext context, IQueryable<TSource> source, IQueryable destination, int batchSize = 100000, int timeout = 0, int notifyFrequency = 1000, IObserver<TSource> observer = null)
        {
            VerifyDestinationCanReceiveSource(source, destination);

            var entityType = context.Model.FindEntityType(destination.ElementType);
            var relational = entityType.Relational();

            var bulkConnection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);

            try
            {
                await bulkConnection.OpenAsync();

                using (var bulkCopy = new SqlBulkCopy(bulkConnection)
                {
                    DestinationTableName = string.IsNullOrWhiteSpace(relational.Schema)
                        ? $"[{relational.TableName}]"
                        : $"[{relational.Schema}].[{relational.TableName}]",
                    BatchSize = batchSize,
                    BulkCopyTimeout = timeout,
                    NotifyAfter = notifyFrequency,
                    EnableStreaming = true
                })
                {
                    bulkCopy.SqlRowsCopied += BulkCopy_SqlRowsCopied;

                    using (var reader = new QueryableDataReader<TSource>(source))
                    {
                        if (observer != null)
                        {
                            var observable = source.ToObservable();
                            observable.Subscribe(observer);
                        }

                        await bulkCopy.WriteToServerAsync(reader);
                    }
                }
            }
            finally
            {
                bulkConnection.Dispose();
            }
        }

        private static void VerifyDestinationCanReceiveSource(IQueryable source, IQueryable destination)
        {
            var sourceFields = source.ElementType.GetProperties();
            var destinationFields = destination.ElementType.GetProperties();

            if (!destinationFields.All(info => sourceFields.Any(propertyInfo => propertyInfo.Name == info.Name && propertyInfo.PropertyType == info.PropertyType)))
            {
                throw new InvalidOperationException("The bulk insert destination is incompatible with the source.");
            }
        }

        private static void BulkCopy_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            BulkHelper.ReportBulkProgress(sender, e);
        }
    }
}
