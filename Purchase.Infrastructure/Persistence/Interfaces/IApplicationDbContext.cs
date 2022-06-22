using Microsoft.EntityFrameworkCore;
using Purchase.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase;

namespace Purchase.Infrastructure.Persistence.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<Commission> Commission { get; set; }
        public DbSet<MobileProduct> MobileProduct { get; set; }
        public DbSet<MobileProductType> MobileProductType { get; set; }
        public DbSet<MobileTransaction> MobileTransaction { get; set; }
        public DbSet<NetworkProvider> NetworkProvider { get; set; }
        public DbSet<Core.Entities.ServiceProvider> ServiceProvider { get; set; }
        public DbSet<SubType> SubType { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
