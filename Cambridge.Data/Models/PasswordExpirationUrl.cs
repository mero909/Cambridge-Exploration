using System;
using System.Collections.Generic;

namespace Cambridge.Data.Models
{
    public partial class PasswordExpirationUrl
    {
        public int ID { get; set; }
        public int ContactID { get; set; }
        public string Url { get; set; }
        public bool IsExpired { get; set; }
        public System.DateTime CreateDate { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
