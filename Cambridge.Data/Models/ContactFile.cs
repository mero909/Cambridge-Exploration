using System;
using System.Collections.Generic;

namespace Cambridge.Data.Models
{
    public partial class ContactFile
    {
        public int ID { get; set; }
        public int ContactID { get; set; }
        public string FileName { get; set; }
        public byte[] FilePasscode { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
