using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.Models;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Models.SP.Query;
using SchoolDBWebAPI.Services.Models.SP.Quiz;
using SchoolDBWebAPI.Services.Services;
using SchoolDBWebAPI.Services.SPHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizDetailController : ControllerBase
    {
        private readonly IQuizDetailService service;
        private readonly ILogger<QuizDetailController> logger;

        public QuizDetailController(IQuizDetailService _quizDetailService, ILogger<QuizDetailController> _logger)
        {
            logger = _logger;
            service = _quizDetailService;
        }

        // GET api/<QuizDetail>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            RequestResponse response = new();

            try
            {
                QuizDetail quizDetail = service.GetByIDAsync(id).Result;

                if (quizDetail != null)
                {
                    response.Success = true;
                    response.Data = quizDetail;
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

        [HttpGet("quiz/{id}")]
        public async Task<IActionResult> GetWithQuesAsync(int id)
        {
            RequestResponse response = new();

            try
            {
                QuizDetail quizDetail = await service.QuizWithQuesAsync(id);

                if (quizDetail != null)
                {
                    response.Success = true;
                    response.Data = quizDetail;
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

        [HttpGet("search/{qry}")]
        public IActionResult GetByName(string qry)
        {
            RequestResponse response = new();

            try
            {
                List<QuizDetail> quizDetail = service.SearchQuizByTitle(qry);

                if (quizDetail != null)
                {
                    response.Success = true;
                    response.Data = quizDetail;
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

        // DELETE api/<QuizDetail>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            RequestResponse response = new();

            try
            {
                response.Success = await service.DeleteByIdAsync(id) > 0;
            }
            catch (Exception Ex)
            {
                response.Success = false;
                response.Message = Ex.Message;
                logger.LogError(Ex, Ex.Message);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("deleteall")]
        public IActionResult DeleteAll()
        {
            RequestResponse response = new();

            try
            {
                response.Success = service.DeleteRange(data => data.Title.Contains("string")) > 0;
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
        [Route("add")]
        public IActionResult Add(SP_QuizDetailInsert model)
        {
            RequestResponse response = new();

            try
            {
                List<DBSQLParameter> paramList = new()
                {
                    new DBSQLParameter("@Title", model.Title),
                    new DBSQLParameter("@EndDate", model.EndDate),
                    new DBSQLParameter("@PaidQuiz", model.PaidQuiz),
                    new DBSQLParameter("@StartDate", model.StartDate),
                    new DBSQLParameter("@CreatorId", model.CreatorId),
                    new DBSQLParameter("@Description", model.Description)
                };

                QuizDetail quizDetail = service.AddQuiz(paramList);

                if (quizDetail != null)
                {
                    response.Success = true;
                    response.Data = quizDetail;
                    response.Message = "Quiz Added Successfully";
                }
                else
                {
                    response.Message = "Failed to Add Quiz Details";
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
        [Route("insert")]
        public IActionResult Insert(QuizDetail model)
        {
            RequestResponse response = new();

            try
            {
                if (service.Insert(model).Id > 0)
                {
                    response.Success = true;
                    response.Message = "Quiz Added Successfully";
                }
                else
                {
                    response.Message = "Failed to Add Quiz Details";
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
        public async Task<IActionResult> UpdateAsync(QuizDetail model)
        {
            RequestResponse response = new();

            try
            {
                if (await service.UpdateAsync(model))
                {
                    response.Success = true;
                    response.Message = "Quiz Updated Successfully";
                }
                else
                {
                    response.Message = "Failed to Updated Quiz Details";
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
        [Route("getall")]
        public IActionResult GetAll(Qry_SP_StudentMasterSelect model)
        {
            RequestResponse response = new();

            try
            {
                List<QuizDetail> QuizDetail = service.ListAllQuiz(model);

                if (QuizDetail != null)
                {
                    response.Success = true;
                    response.Data = QuizDetail;
                    response.Message = "Details Successfully";
                }
                else
                {
                    response.Message = "Failed to Get Details";
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