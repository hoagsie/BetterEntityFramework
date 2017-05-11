using Microsoft.EntityFrameworkCore;

namespace BetterEntityFramework.StoreData
{
    public partial class EfStoreContext
    {
        private readonly DbContextOptions _options;

        public EfStoreContext()
        {

        }

        public EfStoreContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_options == null)
            {
                optionsBuilder.UseSqlServer(@"Server=(local);Database=EfStore;Trusted_Connection=True;");
            }
        }
    }
}
