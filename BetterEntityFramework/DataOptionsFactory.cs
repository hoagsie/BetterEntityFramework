using Microsoft.EntityFrameworkCore;

namespace BetterEntityFramework
{
    internal class DataOptionsBuilder : DbContextOptionsBuilder
    {
        private static DbContextOptionsBuilder _builder;

        public DataOptionsBuilder()
        {
            _builder = new DbContextOptionsBuilder();
        }

        public DataOptionsBuilder(DbContextOptions options)
        {
            _builder = new DbContextOptionsBuilder(options);
        }

        public static DbContextOptions Build()
        {
            return _builder.IsConfigured ? _builder.Options : null;
        }
    }
}
