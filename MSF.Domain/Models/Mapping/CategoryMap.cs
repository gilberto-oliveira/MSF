using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.HasIndex(b => b.Code).IsUnique();
            builder.Property(b => b.Code).HasMaxLength(5).IsRequired();
            builder.Property(b => b.Description).HasMaxLength(256).IsRequired();
        }
    }
}
