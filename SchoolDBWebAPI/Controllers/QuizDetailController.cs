using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.Data.DBHelper;
using SchoolDBWebAPI.Data.Interfaces;
using SchoolDBWebAPI.DBModels;
using SchoolDBWebAPI.Models;
using SchoolDBWebAPI.Models.SP.Query;
using SchoolDBWebAPI.Models.SP.Quiz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SchoolDBWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizDetailController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private IProcedureManager procedureManager;
        private readonly IRepository<QuizDetail> repository;
        private readonly ILogger<QuizDetailController> logger;

        public QuizDetailController(IUnitOfWork _unitOfWork, IProcedureManager _procedureManager, ILogger<QuizDetailController> _logger)
        {
            logger = _logger;
            unitOfWork = _unitOfWork;
            procedureManager = _procedureManager;
            repository = unitOfWork.QuizDetailRepository;
        }

        // GET api/<QuizDetail>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            RequestResponse response = new RequestResponse();

            try
            {
                QuizDetail quizDetail = repository.GetByIDAsync(id).Result;

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
            RequestResponse response = new RequestResponse();

            try
            {
                //List<QuizDetail> quizDetail = repository.Get(data => data.Title.Contains(qry, StringComparison.InvariantCultureIgnoreCase)).ToList();
                //DataTable quizDetail = procedureManager.ExecuteSelect($@"Select * from QuizDetail where Title like '%' + @Qry +'%'", new SqlParameter("@Qry", qry));
                //List<QuizDetail> quizDetail = procedureManager.ExecuteSelect<QuizDetail>($@"Select * from QuizDetail where Id = @Qry", new SqlParameter("@Qry", qry));
                List<QuizDetail> quizDetail = procedureManager.ExecuteSelect<QuizDetail>($@"Select * from QuizDetail where Title like '%' + @Qry +'%'", new SqlParameter("@Qry", qry));

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
        public IActionResult Delete(int id)
        {
            RequestResponse response = new RequestResponse();

            try
            {
                repository.DeleteByIdAsync(id);
                response.Success = unitOfWork.SaveChanges() > 0;
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
            RequestResponse response = new RequestResponse();

            try
            {
                unitOfWork.BeginTransaction();
                repository.DeleteRange(data => data.Title.Contains("string"));
                response.Success = unitOfWork.SaveChanges() > 0;
                unitOfWork.Commit();
                unitOfWork.Rollback();
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
            RequestResponse response = new RequestResponse();

            try
            {
                var paramList = new List<DBSQLParameter>();
                paramList.Add(new DBSQLParameter("@Title", model.Title));
                paramList.Add(new DBSQLParameter("@EndDate", model.EndDate));
                paramList.Add(new DBSQLParameter("@PaidQuiz", model.PaidQuiz));
                paramList.Add(new DBSQLParameter("@StartDate", model.StartDate));
                paramList.Add(new DBSQLParameter("@CreatorId", model.CreatorId));
                paramList.Add(new DBSQLParameter("@Description", model.Description));

                List<QuizDetail> QuizDetail = procedureManager.ExecStoreProcedure<QuizDetail>("SP_QuizDetailInsert", paramList);

                if (QuizDetail != null && QuizDetail.FirstOrDefault() != null)
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
        [Route("getall")]
        public IActionResult GetAll(Qry_SP_StudentMasterSelect model)
        {
            RequestResponse response = new RequestResponse();

            try
            {
                List<QuizDetail> QuizDetail = procedureManager.ExecStoreProcedure<QuizDetail>("SP_StudentMasterSelect", model);

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