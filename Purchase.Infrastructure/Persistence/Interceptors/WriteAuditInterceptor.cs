using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Purchase.Infrastructure.Persistence.DatabaseAudit;
using Purchase.Infrastructure.Persistence.DatabaseAuditModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Infrastructure.Persistence.Interceptors
{
    public class WriteAuditInterceptor : SaveChangesInterceptor
    {
        private static WriteAuditEntity _audit;
        private readonly string _connectionString;
        private readonly WriteAuditContext _writeAuditContext;
        public WriteAuditInterceptor(string connectionString)
        {
            _connectionString = connectionString;
            _writeAuditContext = new WriteAuditContext(_connectionString);
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            _audit = CreateAudit(eventData.Context);

            _writeAuditContext.Add(_audit);
            _writeAuditContext.SaveChangesAsync();

            return result;
        }
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {


            _writeAuditContext.Attach(_audit);
            _audit.Succeeded = true;
            _audit.EndTime = DateTime.UtcNow;
            _writeAuditContext.SaveChanges();

            return result;
        }

        public override void SaveChangesFailed(DbContextErrorEventData eventData)
        {


            _writeAuditContext.Attach(_audit);
            _audit.Succeeded = false;
            _audit.EndTime = DateTime.UtcNow;
            _audit.ErrorMessage = eventData.Exception.Message;

            _writeAuditContext.SaveChanges();
        }

        private static WriteAuditEntity CreateAudit(DbContext context)
        {
            context.ChangeTracker.DetectChanges();

            var audit = new WriteAuditEntity { AuditId = Guid.NewGuid(), StartTime = DateTime.UtcNow };

            foreach (var entry in context.ChangeTracker.Entries())
            {
                var auditMessage = entry.State switch
                {
                    EntityState.Deleted => CreateDeletedMessage(entry),
                    EntityState.Modified => CreateModifiedMessage(entry),
                    EntityState.Added => CreateAddedMessage(entry),
                    _ => null
                };

                if (auditMessage != null)
                {
                    audit.Entities.Add(new EntityAudit { State = entry.State, AuditMessage = auditMessage });
                }
            }

            return audit;

            string CreateAddedMessage(EntityEntry entry)
                => entry.Properties.Aggregate(
                    $"Inserting {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");

            string CreateModifiedMessage(EntityEntry entry)
                => entry.Properties.Where(property => property.IsModified || property.Metadata.IsPrimaryKey()).Aggregate(
                    $"Updating {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");

            string CreateDeletedMessage(EntityEntry entry)
                => entry.Properties.Where(property => property.Metadata.IsPrimaryKey()).Aggregate(
                    $"Deleting {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");
        }








    }
}
