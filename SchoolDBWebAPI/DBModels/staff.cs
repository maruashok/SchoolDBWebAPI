using System;

#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class staff
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StaffCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public int AddressId { get; set; }
        public string Emaild { get; set; }
        public string MobileNo { get; set; }
        public int? Age { get; set; }
        public decimal? Salary { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string IsActive { get; set; }

        public virtual Address Address { get; set; }
    }
}