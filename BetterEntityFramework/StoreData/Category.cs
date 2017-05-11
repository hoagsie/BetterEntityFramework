using System;
using System.Collections.Generic;

namespace BetterEntityFramework.StoreData
{
    public partial class Category
    {
        public Category()
        {
            CategorySubscription = new HashSet<CategorySubscription>();
            CategoryTreeChildCategoryNavigation = new HashSet<CategoryTree>();
            CategoryTreeParentCategoryNavigation = new HashSet<CategoryTree>();
        }

        public int CategoryId { get; set; }
        public Guid SystemId { get; set; }
        public string Name { get; set; }
        public bool Publish { get; set; }

        public virtual ICollection<CategorySubscription> CategorySubscription { get; set; }
        public virtual ICollection<CategoryTree> CategoryTreeChildCategoryNavigation { get; set; }
        public virtual ICollection<CategoryTree> CategoryTreeParentCategoryNavigation { get; set; }
    }
}
