using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Interfaces;
using SchoolDBWebAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository staffRepository;

        public StaffService(IStaffRepository _staffRepository)
        {
            staffRepository = _staffRepository;
        }

        public staff GetStaff(int staffId)
        {
            return staffRepository.GetStaffById(staffId);
        }

        public async Task<bool> UpdateAsync(staff model)
        {
            bool isUpdated = default;

            staff staff = await staffRepository.GetFirstAsync(quiz => quiz.Id == model.Id, quiz => quiz.Address);

            if (staff != null)
            {
                staffRepository.SetEntityValues(staff, model);
                staff.Address = model.Address;
                staffRepository.Update(staff, quiz => quiz.Address);
                isUpdated = await staffRepository.SaveChangesAsync() > 0;
            }

            return isUpdated;
        }
    }
}