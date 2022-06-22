using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchase.Core.Entities;
using Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Infrastructure.Persistence.Configurations
{
    public class MobileTransactionConfiguration : IEntityTypeConfiguration<MobileTransaction>
    {
        public void Configure(EntityTypeBuilder<MobileTransaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TraderId).HasColumnType("nvarchar(50)").IsRequired(true);
            builder.Property(x => x.ServiceProviderUId).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(x => x.ProductDescription).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.Barcode).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.MobileNumber).HasColumnType("nvarchar(20)").IsRequired(false);
            builder.Property(x => x.Cost).HasColumnType("decimal(18,4)").IsRequired(true);
            builder.Property(x => x.SMS).HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.ThumbnailLogo).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.BannerLogo).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.SubTypeDescription).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.MobileProductTypeDescription).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.CategoryDescription).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.ServiceProviderDescription).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.NetworkProviderDescription).HasColumnType("nvarchar(255)").IsRequired(true);
            builder.Property(x => x.CommissionTotal).HasColumnType("decimal(18,4)").IsRequired(true);
            builder.Property(x => x.CommissionThrive).HasColumnType("decimal(18,4)").IsRequired(true);
            builder.Property(x => x.CommissionTrader).HasColumnType("decimal(18,4)").IsRequired(true);
            builder.Property(x => x.CommissionType).HasColumnType("nvarchar(25)").IsRequired(true);
            builder.Property(x => x.CreatedDate).HasColumnType("datetimeoffset(7)").IsRequired(true);
        }
    }
}
