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
    public class MobileProductConfiguration : IEntityTypeConfiguration<MobileProduct>
    {
        public void Configure(EntityTypeBuilder<MobileProduct> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CategoryId).HasColumnType("bigint").IsRequired(true);
            builder.Property(x => x.ServiceProviderId).HasColumnType("bigint").IsRequired(true);
            builder.Property(x => x.SubTypeId).HasColumnType("bigint").IsRequired(true);
            builder.Property(x => x.ServiceProviderUId).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(x => x.NetworkProviderId).HasColumnType("bigint").IsRequired(true);
            builder.Property(x => x.MobileProductTypeId).HasColumnType("bigint").IsRequired(true);
            builder.Property(x => x.Description).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.Barcode).HasColumnType("nvarchar(50)").IsRequired(true);
            builder.Property(x => x.Cost).HasColumnType("decimal(18,4)").IsRequired(true);
            builder.Property(x => x.CommissionId).HasColumnType("bigint").IsRequired(true);
            builder.Property(x => x.SMS).HasColumnType("bit").IsRequired(true);
            builder.Property(x => x.ThumbnailLogo).HasColumnType("nvarchar(255)").IsRequired(false);
            builder.Property(x => x.BannerLogo).HasColumnType("nvarchar(255)").IsRequired(false);
           
            builder.Property(x => x.Enabled).HasColumnType("bit").IsRequired();
            builder.Property(x => x.CreatedDate).HasColumnType("datetimeoffset(7)").IsRequired();
            builder.Property(x => x.ModifiedDate).HasColumnType("datetimeoffset(7)").IsRequired();
            builder.Property(x => x.DeletedDate).HasColumnType("datetimeoffset(7)").IsRequired();
            builder.HasOne(x => x.Category).WithMany(x => x.MobileProducts).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.ServiceProvider).WithMany(x => x.MobileProducts).HasForeignKey(x => x.ServiceProviderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.MobileProductType).WithMany(x => x.MobileProducts).HasForeignKey(x => x.MobileProductTypeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Commission).WithMany(x => x.MobileProducts).HasForeignKey(x => x.CommissionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.NetworkProvider).WithMany(x => x.MobileProducts).HasForeignKey(x => x.NetworkProviderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.SubType).WithMany(x => x.MobileProducts).HasForeignKey(x => x.SubTypeId).OnDelete(DeleteBehavior.Cascade);


        }
    }
}
