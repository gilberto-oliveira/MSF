using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class WorkCenterControlMap : IEntityTypeConfiguration<WorkCenterControl>
    {
        public void Configure(EntityTypeBuilder<WorkCenterControl> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.HasOne(b => b.WorkCenter).WithMany(b => b.WorkCenterControls).HasForeignKey(b => b.WorkCenterId);
        }
    }
}
