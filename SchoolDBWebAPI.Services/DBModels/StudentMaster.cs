using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.Services.DBModels
{
    public partial class StudentMaster
    {
        public StudentMaster()
        {
            EducationDetails = new HashSet<EducationDetail>();
            PaymentInfos = new HashSet<PaymentInfo>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public int AddressId { get; set; }
        public string Emaild { get; set; }
        public string MobileNo { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string IsActive { get; set; }
        public string StudentPic { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<EducationDetail> EducationDetails { get; set; }
        public virtual ICollection<PaymentInfo> PaymentInfos { get; set; }
    }
}
