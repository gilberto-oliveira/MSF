using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class SubcategoryMap : IEntityTypeConfiguration<Subcategory>
    {
        public void Configure(EntityTypeBuilder<Subcategory> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Description).HasMaxLength(256).IsRequired();
            builder.HasOne(b => b.Category).WithMany(b => b.Subcategories).HasForeignKey(b => b.CategoryId);
        }
    }
}
