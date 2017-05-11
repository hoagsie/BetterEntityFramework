using Microsoft.EntityFrameworkCore;

namespace BetterEntityFramework
{
    internal class DataOptionsBuilder
    {
        private static DbContextOptionsBuilder _builder;

        public DbContextOptionsBuilder Builder => _builder;

        public DataOptionsBuilder()
        {
            _builder = new DbContextOptionsBuilder();
        }

        public DataOptionsBuilder(DbContextOptions options)
        {
            _builder = new DbContextOptionsBuilder(options);
        }

        public DbContextOptions Build()
        {
            return _builder.IsConfigured ? _builder.Options : null;
        }

        public static implicit operator DbContextOptionsBuilder(DataOptionsBuilder builder)
        {
            return builder.Builder;
        }

        public static implicit operator DataOptionsBuilder(DbContextOptionsBuilder builder)
        {
            return new DataOptionsBuilder(builder.Options);
        }
    }
}
