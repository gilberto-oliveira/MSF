using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Description).HasMaxLength(256);
            builder.Property(b => b.Profit).HasColumnType("decimal (18,4)");
            builder.HasOne(b => b.Subcategory).WithMany(b => b.Products).HasForeignKey(b => b.SubcategoryId);
        }
    }
}
