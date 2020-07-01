using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSF.Domain.Models.Mapping
{
    public class StateMap : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Name).HasMaxLength(30);
            builder.Property(b => b.Initials).HasMaxLength(3);
        }
    }
}
