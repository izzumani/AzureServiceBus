using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Core.Entities
{
    public class MobileProduct: BaseEntity<long>
    {

        public long CategoryId { get;  set; }

        public long ServiceProviderId { get;  set; }
        public long SubTypeId { get;  set; }
        public string ServiceProviderUId { get;  set; } = string.Empty;
        public long NetworkProviderId { get;  set; }
        public long MobileProductTypeId { get;  set; }
        public string Description { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal Cost { get; set; } = decimal.Zero;
        public long CommissionId { get;  set; }

        public bool SMS { get; set; } = false;

        public string ThumbnailLogo { get; set; } = string.Empty;
        public string BannerLogo { get; set; } = string.Empty;

        public Category? Category { get; private set; }

        public Commission? Commission { get; private set; }

        public MobileProductType? MobileProductType { get; private set; }

        public NetworkProvider? NetworkProvider { get; private set; }

        public ServiceProvider? ServiceProvider { get; private set; }

        public SubType? SubType { get; private set; }

    }
}
