using MSF.Identity.Models;
using MSF.Identity.Models.Mapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MSF.Identity.Context
{
    public class MSFIdentityDbContext : IdentityDbContext<User, Role,
        int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public MSFIdentityDbContext(DbContextOptions<MSFIdentityDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplyConfiguration(builder);
        }

        private void ApplyConfiguration(ModelBuilder builder)
        {
            builder.HasDefaultSchema("System");

            builder.ApplyConfiguration(new UserMap());

            builder.ApplyConfiguration(new RoleMap());

            builder.ApplyConfiguration(new UserClaimMap());

            builder.ApplyConfiguration(new UserLoginMap());

            builder.ApplyConfiguration(new UserTokenMap());

            builder.ApplyConfiguration(new RoleClaimMap());

            builder.ApplyConfiguration(new UserRoleMap());
        }
    }
}
