using SchoolDBWebAPI.Services.DBModels;

namespace SchoolDBWebAPI.Services.Repository
{
    public interface IStaffRepository : IRepository<staff>
    {
        public staff GetStaffById(int id);
    }

    public class StaffRepository : BaseRepository<staff>, IStaffRepository
    {

        public StaffRepository(SchoolDBContext dBContext):base(dBContext)
        {
        }

        public staff GetStaffById(int id)
        {
            return GetFirst(staff => staff.Id == id, includeProperties: "Address");
        }
    }
}
