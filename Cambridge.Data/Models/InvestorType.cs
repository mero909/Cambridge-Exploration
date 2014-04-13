using System;
using System.Collections.Generic;

namespace Cambridge.Data.Models
{
    public partial class InvestorType
    {
        public InvestorType()
        {
            this.Contacts = new List<Contact>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
