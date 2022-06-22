using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase.Infrastructure.Persistence.Interfaces;
using Purchase.Core.Entities;
using System.Reflection;

namespace Purchase.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        

        public ApplicationDbContext(
            DbContextOptions options) : base(options)
        {

            

        }


        public DbSet<Category> Category { get; set; }
        public DbSet<Commission> Commission { get; set; }
        public DbSet<MobileProduct> MobileProduct { get; set; }
        public DbSet<MobileProductType> MobileProductType { get; set; }
        public DbSet<MobileTransaction> MobileTransaction { get; set; }
        public DbSet<NetworkProvider> NetworkProvider { get; set; }
        public DbSet<Core.Entities.ServiceProvider> ServiceProvider { get; set; }
        public DbSet<SubType> SubType { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Todo Any changes before saving, including audit

            var result = await base.SaveChangesAsync(cancellationToken);

            // any changes after the savings. You can apply events, audit or cache
           
            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(builder);
        }

       

    }
}
