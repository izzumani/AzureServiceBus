    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Core.Entities
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }

        public bool Enabled { get; set; } = false;

        public DateTimeOffset? CreatedDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? ModifiedDate { get; set;}

        public DateTimeOffset? DeletedDate { get; set; }

    

    }
}
