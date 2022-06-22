using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Infrastructure.Persistence.DatabaseAuditModels
{
    public class WriteAuditEntity
    {
        public int Id { get; set; }
        public Guid AuditId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }

        public ICollection<EntityAudit> Entities { get; } = new List<EntityAudit>();
    }
}
