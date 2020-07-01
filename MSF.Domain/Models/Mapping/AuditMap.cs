using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class AuditMap : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audits", "Security");
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.DateTime).HasDefaultValueSql("GETDATE()");
        }
    }
}
