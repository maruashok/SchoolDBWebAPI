using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.Services.DBModels
{
    public partial class UserMaster
    {
        public UserMaster()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Emaild { get; set; }
        public string Password { get; set; }
        public string ProfilePicPath { get; set; }
        public string IsVerified { get; set; }
        public int? UserType { get; set; }
        public int? Hidden { get; set; }
        public int? MasterId { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
