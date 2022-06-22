using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Infrastructure.Persistence.DatabaseAuditModels
{
    public class EntityAudit
    {
        public int Id { get; set; }
        public EntityState State { get; set; }
        public string AuditMessage { get; set; }

        public WriteAuditEntity SaveChangesAudit { get; set; }
    }
}
