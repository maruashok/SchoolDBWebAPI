using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.DAL.DBModels
{
    public partial class Address
    {
        public Address()
        {
            StudentMasters = new HashSet<StudentMaster>();
            staff = new HashSet<staff>();
        }

        public int Id { get; set; }
        public string Details { get; set; }
        public string Pincode { get; set; }
        public int? City { get; set; }
        public int? State { get; set; }
        public int? Country { get; set; }

        public virtual ICollection<StudentMaster> StudentMasters { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
