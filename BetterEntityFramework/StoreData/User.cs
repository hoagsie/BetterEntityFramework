using System;
using System.Collections.Generic;

namespace BetterEntityFramework.StoreData
{
    public partial class User
    {
        public User()
        {
            UserBasket = new HashSet<UserBasket>();
        }

        public int UserId { get; set; }
        public Guid Identity { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ContactPreference { get; set; }
        public bool Validated { get; set; }
        public bool InCollections { get; set; }
        public bool Inactive { get; set; }

        public virtual ICollection<UserBasket> UserBasket { get; set; }
    }
}
