using SchoolDBWebAPI.DAL.DBModels;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface IStaffService
    {
        staff GetStaff(int staffId);

        Task<bool> UpdateAsync(staff model);
    }
}