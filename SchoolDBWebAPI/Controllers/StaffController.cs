using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Models;
using System;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService service;

        public StaffController(IStaffService staffService)
        {
            service = staffService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(int id)
        {
            RequestResponse response = new();

            staff staffDetail = service.GetStaff(id);

            if (staffDetail != null)
            {
                response.Success = true;
                response.Data = staffDetail;
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(staff model)
        {
            RequestResponse response = new();

            if (await service.UpdateAsync(model))
            {
                response.Success = true;
                response.Message = "Staff Updated Successfully";
            }
            else
            {
                response.Message = "Failed to Updated Staff Details";
            }

            return Ok(response);
        }
    }
}