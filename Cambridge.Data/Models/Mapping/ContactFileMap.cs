using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Cambridge.Data.Models.Mapping
{
    public class ContactFileMap : EntityTypeConfiguration<ContactFile>
    {
        public ContactFileMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.FileName)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(t => t.FilePasscode)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ContactFile");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ContactID).HasColumnName("ContactID");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.FilePasscode).HasColumnName("FilePasscode");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            // Relationships
            this.HasRequired(t => t.Contact)
                .WithMany(t => t.ContactFiles)
                .HasForeignKey(d => d.ContactID);

        }
    }
}
