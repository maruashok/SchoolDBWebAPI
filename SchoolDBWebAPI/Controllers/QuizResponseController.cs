using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.Models;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Services;
using System;

namespace SchoolDBWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizResponseController : ControllerBase
    {
        private readonly IQuizResultService service;
        private readonly ILogger<QuizResponseController> logger;

        public QuizResponseController(IQuizResultService _service, ILogger<QuizResponseController> _logger)
        {
            this.logger = _logger;
            this.service = _service;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            RequestResponse response = new();

            try
            {
                QuizResponse quizResponse = service.GetByIDAsync(id).Result;

                if (quizResponse != null)
                {
                    response.Success = true;
                    response.Data = quizResponse;
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

        [HttpGet("Quiz/{QuizId}")]
        public IActionResult QuizResponses(int QuizId)
        {
            RequestResponse response = new();

            try
            {
                var quizResponse = service.GetQuizResponses(QuizId);

                if (quizResponse != null)
                {
                    response.Success = true;
                    response.Data = quizResponse;
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
    }
}