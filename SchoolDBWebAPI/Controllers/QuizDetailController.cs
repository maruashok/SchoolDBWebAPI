using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Models.SP.Query;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizDetailController : ControllerBase
    {
        private readonly IQuizDetailService service;

        public QuizDetailController(IQuizDetailService _quizDetailService)
        {
            service = _quizDetailService;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            RequestResponse response = new();

            QuizDetail quizDetail = service.GetByID(id);

            if (quizDetail != null)
            {
                response.Success = true;
                response.Data = quizDetail;
            }

            return Ok(response);
        }

        [HttpGet("{id}/quesAsync")]
        public async Task<IActionResult> GetWithQuesAsync(int id)
        {
            RequestResponse response = new();

            QuizDetail quizDetail = await service.QuizWithQuesAsync(id);

            if (quizDetail != null)
            {
                response.Success = true;
                response.Data = quizDetail;
            }

            return Ok(response);
        }

        [HttpGet("{id}/ques")]
        public IActionResult GetWithQues(int id)
        {
            RequestResponse response = new();

            var quizDetail = service.QuizWithQues(id);

            if (quizDetail != null)
            {
                response.Success = true;
                response.Data = quizDetail;
            }

            return Ok(response);
        }

        [HttpGet("searchTitle")]
        public IActionResult GetByName(string qry)
        {
            RequestResponse response = new();

            List<QuizDetail> quizDetail = service.SearchQuizByTitle(qry);

            if (quizDetail != null)
            {
                response.Success = true;
                response.Data = quizDetail;
            }

            return Ok(response);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            RequestResponse response = new();

            response.Success = service.DeleteByID(id);

            return Ok(response);
        }

        [HttpPut]
        public IActionResult Insert(QuizDetail model)
        {
            RequestResponse response = new();

            if (service.Insert(model))
            {
                response.Success = true;
                response.Message = "Quiz Added Successfully";
            }
            else
            {
                response.Message = "Failed to Add Quiz Details";
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("updateAsync")]
        public async Task<IActionResult> UpdateAsync(QuizDetail model)
        {
            RequestResponse response = new();

            if (await service.UpdateAsync(model))
            {
                response.Success = true;
                response.Message = "Quiz Updated Successfully";
            }
            else
            {
                response.Message = "Failed to Updated Quiz Details";
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("getall")]
        public IActionResult GetAll(Qry_SP_StudentMasterSelect model)
        {
            RequestResponse response = new();

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

            return Ok(response);
        }
    }
}