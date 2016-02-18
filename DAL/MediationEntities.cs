using System.Configuration;
using System.Data.Entity;
using DAL.Mappings;
using Domain;

namespace DAL
{
    public class MediationEntities : DbContext
    {
        public MediationEntities()
        {
            Configuration.ProxyCreationEnabled = true;
        }

        public MediationEntities(string connectionString) : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMapping());
        }

        protected override void Dispose(bool disposing)
        {
            Configuration.LazyLoadingEnabled = false;
            base.Dispose(disposing);
        }
    }
}