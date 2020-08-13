using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Identity.Models.Mapping
{
    public class UserRoleShopMap : IEntityTypeConfiguration<UserRoleShop>
    {
        public void Configure(EntityTypeBuilder<UserRoleShop> builder)
        {
            builder.ToTable("UserRoleShops");
            builder.HasOne(b => b.UserRole).WithMany(b => b.UserRoleShops).HasForeignKey(b => new { b.UserId, b.RoleId });
        }
    }
}
