using System;
using System.Collections.Generic;

namespace BetterEntityFramework.StoreData
{
    public partial class ProductBundle
    {
        public int BundleId { get; set; }
        public Guid RootProduct { get; set; }
        public string RootSku { get; set; }
        public Guid AssociatedProduct { get; set; }
        public string AssociatedSku { get; set; }
        public int AssociatedQuantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Product AssociatedProductNavigation { get; set; }
        public virtual Product RootProductNavigation { get; set; }
    }
}
