using System;

namespace SchoolDBWebAPI.Services.Models.SP
{
    public class SP_StudentMasterSelect
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? AddressId { get; set; }
        public string EMaild { get; set; }
        public string MobileNo { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string IsActive { get; set; }
        public string CityName { get; set; }
        public string StudentPic { get; set; }
    }
}