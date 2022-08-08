using SchoolDBWebAPI.DAL.DBModels;

namespace SchoolDBWebAPI.DAL.Interfaces
{
    public interface IStaffRepository : IBaseRepository<staff>
    {
        public staff GetStaffById(int id);
    }
}