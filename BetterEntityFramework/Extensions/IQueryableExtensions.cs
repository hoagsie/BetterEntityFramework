using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace BetterEntityFramework.Extensions
{
    internal static class IQueryableExtensions
    {
        public static async Task UpdateWhere<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update, CancellationToken ct = default(CancellationToken))
            where TEntity : class
        {
            await query.Where(where).UpdateAsync(update, ct);
        }

        public static async Task DeleteWhere<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> where, CancellationToken ct = default(CancellationToken))
            where TEntity : class
        {
            await query.Where(where).DeleteAsync(ct);
        }
    }
}
