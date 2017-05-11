using System;

namespace BetterEntityFramework.StoreData
{
    public partial class UserBasket
    {
        public int BasketId { get; set; }
        public Guid User { get; set; }
        public Guid Product { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }

        public virtual Product ProductNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
