using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Interfaces;

namespace SchoolDBWebAPI.DAL.Repository
{
    public interface IStaffRepository : IRepository<staff>
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
            return GetFirst(staff => staff.Id == id, includeProperties: "Address");
        }
    }
}