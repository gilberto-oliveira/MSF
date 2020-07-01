using MSF.Domain.Models;
using MSF.Domain.Models.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MSF.Domain.Context
{
    public class MSFDbContext : DbContext, IMSFDbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Subcategory> Subcategories { get; set; }

        public DbSet<WorkCenter> WorkCenters { get; set; }

        public DbSet<Audit> Audits { get; set; }

        public DbSet<WorkCenterControl> WorkCenterControls { get; set; }

        public int? _currentUserId { get; }

        public MSFDbContext(DbContextOptions<MSFDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            // TODO Refatorar para incluir redirecionamento caso o token seja inválido.
            if (httpContextAccessor != null)
            {
                if (httpContextAccessor.HttpContext != null)
                {
                    var name = httpContextAccessor.HttpContext.User.Identity.Name;

                    if (name != null)
                    {
                        if (name.All(Char.IsDigit))
                            _currentUserId = int.Parse(httpContextAccessor.HttpContext.User.Identity.Name);
                        else
                            _currentUserId = 0;
                    }
                    else
                        _currentUserId = 0;
                }
                else
                {
                    _currentUserId = 0;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyConfiguration(modelBuilder);
        }

        private void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ForSqlServerUseIdentityColumns();

            modelBuilder.HasDefaultSchema("MSF");

            modelBuilder.ApplyConfiguration(new CategoryMap());

            modelBuilder.ApplyConfiguration(new SubcategoryMap());

            modelBuilder.ApplyConfiguration(new ProductMap());

            modelBuilder.ApplyConfiguration(new OperationMap());

            modelBuilder.ApplyConfiguration(new ProviderMap());

            modelBuilder.ApplyConfiguration(new ShopMap());

            modelBuilder.ApplyConfiguration(new StockMap());

            modelBuilder.ApplyConfiguration(new StateMap());

            modelBuilder.ApplyConfiguration(new AuditMap());

            modelBuilder.ApplyConfiguration(new WorkCenterMap());

            modelBuilder.ApplyConfiguration(new WorkCenterControlMap());
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await OnAfterSaveChanges(auditEntries);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Metadata.Relational().TableName;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Audits.Add(auditEntry.ToAudit(_currentUserId.Value));
            }

            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                Audits.Add(auditEntry.ToAudit(_currentUserId.Value));
            }

            return SaveChangesAsync();
        }

        public int CommitChanges()
        {
            return SaveChanges();
        }

        public Task<int> CommitChangesAsync()
        {
            return SaveChangesAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }

    public interface IMSFDbContext
    {
        DbSet<Category> Categories { get; set; }

        DbSet<Operation> Operations { get; set; }

        DbSet<Product> Products { get; set; }

        DbSet<Shop> Shops { get; set; }

        DbSet<Provider> Providers { get; set; }

        DbSet<State> States { get; set; }

        DbSet<Stock> Stocks { get; set; }

        DbSet<Subcategory> Subcategories { get; set; }

        DbSet<WorkCenter> WorkCenters { get; set; }

        DbSet<Audit> Audits { get; set; }

        DbSet<WorkCenterControl> WorkCenterControls { get; set; }

        int? _currentUserId { get; }

        int CommitChanges();

        Task<int> CommitChangesAsync();

        void Dispose();
    }

    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }

        public string TableName { get; set; }

        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();

        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();

        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public Audit ToAudit(int UserId)
        {
            var audit = new Audit();
            audit.UserId = UserId;
            audit.Action = StateToString();
            audit.TableName = TableName;
            audit.DateTime = DateTime.UtcNow;
            audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            return audit;
        }

        private String StateToString()
        {
            string state;

            switch (Entry.State)
            {
                case EntityState.Added:
                    state = "Add";
                    break;
                case EntityState.Deleted:
                    state = "Delete";
                    break;
                case EntityState.Modified:
                    state = "Modify";
                    break;
                case EntityState.Unchanged:
                    state = "Add";
                    break;
                default:
                    state = "Any";
                    break;
            }
            return state;
        }
    }
}
