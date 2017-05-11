using System;
using System.Collections.Generic;

namespace BetterEntityFramework.StoreData
{
    public partial class ProductSku
    {
        public int SkuId { get; set; }
        public Guid Product { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public bool Bundled { get; set; }

        public virtual Product ProductNavigation { get; set; }
    }
}
