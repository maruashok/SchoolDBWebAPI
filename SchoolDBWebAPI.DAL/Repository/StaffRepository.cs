using SchoolDBWebAPI.DAL.DBModels;

namespace SchoolDBWebAPI.DAL.Repository
{
    public interface IStaffRepository : IBaseRepository<staff>
    {
        public staff GetStaffById(int id);
    }

    public class StaffRepository : BaseRepository<staff>, IStaffRepository
    {
        public StaffRepository(SchoolDBContext dBContext) : base(dBContext)
        {
        }

        public staff GetStaffById(int id)
        {
            return GetFirst(staff => staff.Id == id, staff => staff.Address);
        }
    }
}