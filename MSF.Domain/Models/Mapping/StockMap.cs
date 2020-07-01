using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class StockMap : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.UnitPrice).HasColumnType("decimal (18,4)");

            builder.HasOne(b => b.Provider).WithMany(b => b.Stocks).HasForeignKey(b => b.ProviderId);
            builder.HasOne(b => b.Product).WithMany(b => b.Stocks).HasForeignKey(b => b.ProductId);
        }
    }
}
