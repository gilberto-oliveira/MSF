using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MSF.Identity.Models
{
    public class UserRole: IdentityUserRole<int>
    {
        public IList<UserRoleShop> UserRoleShops { get; set; }

        public UserRole()
        {
            UserRoleShops = new List<UserRoleShop>();
        }
    }
}
