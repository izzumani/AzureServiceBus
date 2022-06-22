using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Core.Entities
{
    public class MobileTransaction
    {
        public Guid Id { get; set; }
        public string TraderId { get; set; } = string.Empty;
        public string ServiceProviderUId { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;

        public string Barcode { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;    
        public decimal Cost {get; set; } = decimal.Zero;
        public bool? SMS { get; set; } = false;
        public string ThumbnailLogo { get;set; } = string.Empty;
        public string BannerLogo { get; set; } = string.Empty;
        public string SubTypeDescription { get; set; } = string.Empty;
        public string MobileProductTypeDescription { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public string ServiceProviderDescription { get; set; } = string.Empty;
        public string NetworkProviderDescription { get; set; } = string.Empty;
        public decimal CommissionTotal { get; set; } = decimal.Zero;
        public decimal CommissionThrive { get; set; } = decimal.Zero;
        public decimal CommissionTrader { get; set; } = decimal.Zero;
        public string CommissionType { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}
