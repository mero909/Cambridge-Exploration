using System.Data.Entity;
using Cambridge.Data.Models.Mapping;

namespace Cambridge.Data.Models
{
    public class CambridgeContext : DbContext
    {
        static CambridgeContext()
        {
            Database.SetInitializer<CambridgeContext>(null);
        }

        public CambridgeContext()
            : base("Name=CambridgeContext")
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactFile> ContactFiles { get; set; }
        public DbSet<InvestorType> InvestorTypes { get; set; }
        public DbSet<PasswordExpirationUrl> PasswordExpirationUrls { get; set; }
        public DbSet<State> States { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new ContactFileMap());
            modelBuilder.Configurations.Add(new InvestorTypeMap());
            modelBuilder.Configurations.Add(new PasswordExpirationUrlMap());
            modelBuilder.Configurations.Add(new StateMap());
        }
    }
}
