using System;
using System.Collections.Generic;

namespace BetterEntityFramework.StoreData
{
    public partial class CategoryTree
    {
        public int TreeId { get; set; }
        public Guid ParentCategory { get; set; }
        public Guid ChildCategory { get; set; }

        public virtual Category ChildCategoryNavigation { get; set; }
        public virtual Category ParentCategoryNavigation { get; set; }
    }
}
