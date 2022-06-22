using Microsoft.EntityFrameworkCore;
using Purchase.Infrastructure.Persistence.DatabaseAuditModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Infrastructure.Persistence.DatabaseAudit
{
        public class WriteAuditContext : DbContext
        {

            private readonly string _connectionString;

            public WriteAuditContext(string connectionString)
            {
                _connectionString = connectionString;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseSqlite(_connectionString);

            public DbSet<WriteAuditEntity> WriteAuditEntities { get; set; }





        }
    
}
