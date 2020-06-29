using Microsoft.AspNetCore.Identity;

namespace MSF.Identity.Models
{
    public class User: IdentityUser<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
