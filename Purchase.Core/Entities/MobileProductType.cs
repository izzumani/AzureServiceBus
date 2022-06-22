using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Core.Entities
{
    public class MobileProductType : BaseEntity<long>
    {
        public string Description { get; set; } = string.Empty;
        public List<MobileProduct>? MobileProducts { get; set; }
    }
}
