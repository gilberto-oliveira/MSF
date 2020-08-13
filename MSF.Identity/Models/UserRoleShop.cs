using System;
using System.Collections.Generic;
using System.Text;

namespace MSF.Identity.Models
{
    public class UserRoleShop
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public int ShopId { get; set; }

        public UserRole UserRole { get; set; }
    }
}
