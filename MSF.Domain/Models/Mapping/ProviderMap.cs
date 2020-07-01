using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class ProviderMap : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Name).HasMaxLength(30);
            builder.Property(b => b.Code).HasMaxLength(120);
            builder.HasOne(b => b.State).WithMany(b => b.Providers).HasForeignKey(b => b.StateId);
        }
    }
}
