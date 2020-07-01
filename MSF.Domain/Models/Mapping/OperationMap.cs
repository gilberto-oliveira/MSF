using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSF.Domain.Models.Mapping
{
    public class OperationMap : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Type).HasMaxLength(2);
            builder.Property(b => b.UnitPrice).HasColumnType("decimal (18,4)");

            builder.HasOne(b => b.Product).WithMany(b => b.Operations).HasForeignKey(b => b.ProductId);
            builder.HasOne(b => b.WorkCenterControl).WithMany(b => b.Operations).HasForeignKey(b => b.WorkCenterControlId);
            builder.HasOne(b => b.Provider).WithMany(b => b.Operations).HasForeignKey(b => b.ProviderId);
        }
    }
}
