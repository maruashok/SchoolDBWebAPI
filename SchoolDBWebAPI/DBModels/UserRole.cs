#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual RoleMaster Role { get; set; }
        public virtual UserMaster User { get; set; }
    }
}