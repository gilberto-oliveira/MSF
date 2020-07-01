using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class WorkCenterMap : IEntityTypeConfiguration<WorkCenter>
    {
        public void Configure(EntityTypeBuilder<WorkCenter> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Code).HasMaxLength(10);
            builder.Property(b => b.Description).HasMaxLength(30);
            builder.HasOne(b => b.Shop).WithMany(b => b.WorkCenters).HasForeignKey(b => b.ShopId);
        }
    }
}
