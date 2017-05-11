using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BetterEntityFramework.Helpers
{
    internal class BulkHelper
    {
        public static event EventHandler<SqlRowsCopiedEventArgs> InsertProgressReport;

        private static void OnInsertProgressReport(object sender, SqlRowsCopiedEventArgs e)
        {
            InsertProgressReport?.Invoke(sender, e);
        }

        public static void ReportBulkProgress(object sender, SqlRowsCopiedEventArgs rowsCopied)
        {
            Task.Run(() => OnInsertProgressReport(sender, rowsCopied));
        }
    }
}
