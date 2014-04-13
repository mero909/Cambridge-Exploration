using System;
using System.Collections.Generic;

namespace Cambridge.Data.Models
{
    public partial class Contact
    {
        public Contact()
        {
            this.ContactFiles = new List<ContactFile>();
            this.PasswordExpirationUrls = new List<PasswordExpirationUrl>();
            this.InvestorTypes = new List<InvestorType>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] Passcode { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public string PostalCode { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<ContactFile> ContactFiles { get; set; }
        public virtual ICollection<PasswordExpirationUrl> PasswordExpirationUrls { get; set; }
        public virtual ICollection<InvestorType> InvestorTypes { get; set; }
    }
}
