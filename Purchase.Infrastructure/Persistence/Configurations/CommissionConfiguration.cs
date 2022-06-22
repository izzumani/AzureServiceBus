using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchase.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Infrastructure.Persistence.Configurations
{
    public class CommissionConfiguration : IEntityTypeConfiguration<Commission>
    {
        public void Configure(EntityTypeBuilder<Commission> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Total).HasColumnType("decimal(18,4)").IsRequired(true);
            builder.Property(x => x.Thrive).HasColumnType("decimal(18,4)").IsRequired(true);
            builder.Property(x => x.Trader).HasColumnType("decimal(18,4)").IsRequired(true);
            builder.Property(x => x.Type).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(x => x.Enabled).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.CreatedDate).HasColumnType("datetimeoffset(7)").IsRequired(true);
            builder.Property(x => x.ModifiedDate).HasColumnType("datetimeoffset(7)").IsRequired(true);
            builder.Property(x => x.DeletedDate).HasColumnType("datetimeoffset(7)").IsRequired(true);

        }
    }
}
