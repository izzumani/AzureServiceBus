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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).HasColumnType("nvarchar(255)").IsRequired(false);
            builder.Property(x => x.Enabled).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.Logo).HasColumnType("nvarchar(255)").IsRequired(false);
            builder.Property(x => x.CreatedDate).HasColumnType("datetimeoffset(7)").IsRequired(true);
            builder.Property(x => x.ModifiedDate).HasColumnType("datetimeoffset(7)").IsRequired(true);
            builder.Property(x => x.DeletedDate).HasColumnType("datetimeoffset(7)").IsRequired(true);

        }
    }
}
