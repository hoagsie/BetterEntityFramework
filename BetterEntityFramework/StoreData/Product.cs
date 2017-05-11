using System;
using System.Collections.Generic;

namespace BetterEntityFramework.StoreData
{
    public partial class Product
    {
        public Product()
        {
            CategorySubscription = new HashSet<CategorySubscription>();
            ProductBundleAssociatedProductNavigation = new HashSet<ProductBundle>();
            ProductBundleRootProductNavigation = new HashSet<ProductBundle>();
            ProductSku = new HashSet<ProductSku>();
            UserBasket = new HashSet<UserBasket>();
        }

        public int ProductId { get; set; }
        public Guid SystemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Publish { get; set; }

        public virtual ICollection<CategorySubscription> CategorySubscription { get; set; }
        public virtual ICollection<ProductBundle> ProductBundleAssociatedProductNavigation { get; set; }
        public virtual ICollection<ProductBundle> ProductBundleRootProductNavigation { get; set; }
        public virtual ICollection<ProductSku> ProductSku { get; set; }
        public virtual ICollection<UserBasket> UserBasket { get; set; }
    }
}
