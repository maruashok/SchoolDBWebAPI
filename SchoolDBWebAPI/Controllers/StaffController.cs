using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.Models;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Services;
using System;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService service;
        private readonly ILogger<StaffController> logger;

        public StaffController(IStaffService staffService, ILogger<StaffController> _logger)
        {
            logger = _logger;
            service = staffService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            RequestResponse response = new();

            try
            {
                staff staffDetail = service.GetStaff(id);

                if (staffDetail != null)
                {
                    response.Success = true;
                    response.Data = staffDetail;
                    logger.LogInformation("Details loaded");
                }
            }
            catch (Exception Ex)
            {
                response.Success = false;
                response.Message = Ex.Message;
                logger.LogError(Ex, Ex.Message);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateAsync(staff model)
        {
            RequestResponse response = new();

            try
            {
                if (await service.UpdateAsync(model))
                {
                    response.Success = true;
                    response.Message = "Staff Updated Successfully";
                }
                else
                {
                    response.Message = "Failed to Updated Staff Details";
                }
            }
            catch (Exception Ex)
            {
                response.Success = false;
                response.Message = Ex.Message;
                logger.LogError(Ex, Ex.Message);
            }

            return Ok(response);
        }
    }
}