using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Repository;
using Serilog;
using System;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public interface IStaffService
    {
        staff GetStaff(int staffId);

        Task<bool> UpdateAsync(staff model);
    }

    public class StaffService : IStaffService
    {
        private readonly ILogger logger;
        private readonly IStaffRepository staffRepository;

        public StaffService(IStaffRepository _staffRepository)
        {
            staffRepository = _staffRepository;
            logger = Log.ForContext<StaffService>();
        }

        public staff GetStaff(int staffId)
        {
            return staffRepository.GetStaffById(staffId);
        }

        public async Task<bool> UpdateAsync(staff model)
        {
            bool isUpdated = default;

            try
            {
                staff staffDetail = await staffRepository.GetFirstAsync(quiz => quiz.Id == model.Id, includeProperties: "Address");

                if (staffDetail != null)
                {
                    staffRepository.SetEntityValues(staffDetail, model);
                    staffDetail.Address = model.Address;
                    //staffRepository.Update(staffDetail);
                    staffRepository.Update(staffDetail, "Address");
                    isUpdated = await staffRepository.SaveChangesAsync() > 0;
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return isUpdated;
        }
    }
}