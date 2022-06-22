using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Core.Entities
{
    public  class Commission: BaseEntity<long>
    {
        public decimal Total { get; set; } = decimal.Zero;
        public decimal Thrive { get; set; } = decimal.Zero;

        public decimal Trader { get; set; } = decimal.Zero;

        public string Type { get; set; } = string.Empty;
        public List<MobileProduct>? MobileProducts { get; set; }

    }
}
