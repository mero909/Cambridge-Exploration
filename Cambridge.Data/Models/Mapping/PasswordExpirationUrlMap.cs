using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Cambridge.Data.Models.Mapping
{
    public class PasswordExpirationUrlMap : EntityTypeConfiguration<PasswordExpirationUrl>
    {
        public PasswordExpirationUrlMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Url)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("PasswordExpirationUrl");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ContactID).HasColumnName("ContactID");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.IsExpired).HasColumnName("IsExpired");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            // Relationships
            this.HasRequired(t => t.Contact)
                .WithMany(t => t.PasswordExpirationUrls)
                .HasForeignKey(d => d.ContactID);

        }
    }
}
