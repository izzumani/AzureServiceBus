using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.DTO.Aggregates
{
    public class MobileProviderAndBarcode
    {
        public long Id { get; set; } 
        public string Barcode { get; set; }
        public string MobileNetwork { get; set; }
    }
}
