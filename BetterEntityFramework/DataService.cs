using BetterEntityFramework.StoreData;
using Microsoft.EntityFrameworkCore;

namespace BetterEntityFramework
{
    internal class DataService
    {
        private EfStoreContext _data;
        private DbContextOptions _lastOptions;

        /// <summary>
        /// Gets an instance of the data service.
        /// </summary>
        public EfStoreContext Service
        {
            get
            {
                if (_data != null)
                {
                    return _data;
                }

                _lastOptions = DataOptionsBuilder.Build();
                _data = new EfStoreContext(_lastOptions);

                return _data;
            }
        }

        public DataOptionsBuilder Configure()
        {
            return new DataOptionsBuilder();
        }

        public void UseConfiguration(DataOptionsBuilder builder)
        {
            _data?.Dispose();

            _data = new EfStoreContext(builder.Options);
        }

        public DataService WithScopedService()
        {
            var scopedService = new DataService();
            scopedService.UseConfiguration(new DataOptionsBuilder(_lastOptions));
            return scopedService;
        }

        public DataService WithScopedService(DataOptionsBuilder builder)
        {
            var scopedService = new DataService();
            scopedService.UseConfiguration(builder);
            return scopedService;
        }
    }
}
