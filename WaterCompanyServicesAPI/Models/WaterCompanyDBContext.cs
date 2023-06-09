using Microsoft.EntityFrameworkCore;
using ModelLibrary;

namespace WaterCompanyServicesAPI
{
    public class WaterCompanyDBContext:DbContext
    {
        public WaterCompanyDBContext(DbContextOptions<WaterCompanyDBContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.GetForeignKeys()
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.NoAction);
            }

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestDetails> RequestsDetails { get; set; }       
        public DbSet<RequestsLog> RequestsLogs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<User> Users { get; set; }       

    }
}
