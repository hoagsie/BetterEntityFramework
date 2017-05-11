using System;

namespace BetterEntityFramework.StoreData
{
    public partial class CategorySubscription
    {
        public int SubscriptionId { get; set; }
        public Guid Category { get; set; }
        public Guid Product { get; set; }

        public virtual Category CategoryNavigation { get; set; }
        public virtual Product ProductNavigation { get; set; }
    }
}
