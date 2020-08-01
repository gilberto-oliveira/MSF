using System;
using System.Collections.Generic;

namespace MSF.Domain.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public String UserName { get; set; }
        
        public String Password { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }

    public class UsersRefreshRequestViewModel
    {
        public String AccessToken { get; set; }

        public String RefreshToken { get; set; }
    }

    public class UserChangePasswordViewModel
    {
        public String Email { get; set; }

        public String CurrentPassword { get; set; }

        public String NewPassword { get; set; }
    }

    public class LazyUserViewModel
    {
        public int Count { get; set; }
        public IList<UserViewModel> Users { get; set; }
    }

}
