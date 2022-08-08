using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Extensions;
using SchoolDBWebAPI.DAL.Interfaces;
using SchoolDBWebAPI.DAL.Models.SP.Query;
using SchoolDBWebAPI.DAL.SPHelper;

namespace SchoolDBWebAPI.DAL.Repository
{
    public class QuizRepository : BaseRepository<QuizDetail>, IQuizRepository
    {
        private readonly ILogger logger;

        public QuizRepository(ILogger<QuizRepository> _logger, SchoolDBContext dBContext) : base(dBContext)
        {
            logger = _logger;
        }

        public QuizDetail AddQuiz(List<DBSQLParameter> SQLParams)
        {
            return ExecStoreProcedure<QuizDetail>("SP_QuizDetailInsert", SQLParams)?.FirstOrDefault();
        }

        public bool DeleteByID(int QuizId)
        {
            DeleteById(QuizId);
            return SaveChanges() > 0;
        }

        public List<QuizDetail> GetAll(Qry_SP_StudentMasterSelect model)
        {
            return ExecStoreProcedure<QuizDetail>("SP_StudentMasterSelect", model);
        }

        public bool IsQuizExists(int QuizId)
        {
            bool IsExists = false;

            try
            {
                IsExists = GetFirst(quiz => quiz.Id == QuizId) != null;
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex, Ex.Message);
            }

            return IsExists;
        }

        public QuizDetail QuizWithQues(int QuizId)
        {
            var paramList = new List<DBSQLParameter>();
            paramList.Add(new DBSQLParameter("@QuizId", QuizId));
            var result = ExecStoreProcedureMulResults<QuizDetail, QuizQuestion>("SP_QuizWithQues", paramList);
            return (result.Item1.Map(result.Item2, quiz => quiz.Id, quizQues => quizQues.QuizId, (quiz, ques) => { quiz.QuizQuestions = ques.ToList(); }) as List<QuizDetail>).FirstOrDefault();
        }

        public async Task<QuizDetail> QuizWithQuesAsync(int QuizId)
        {
            return await GetFirstAsync(quiz => quiz.Id == QuizId, quiz => quiz.QuizQuestions);
        }

        public List<QuizDetail> SearchQuizByTitle(string QuizTitle)
        {
            return GetWithRawSql($@"Select * from QuizDetail where Title like '%' + @Qry +'%'", QuizTitle).ToList();
        }

        public async Task<bool> UpdateAsync(QuizDetail model)
        {
            QuizDetail quizDetail = await GetFirstAsync(quiz => quiz.Id == model.Id, quiz => quiz.QuizQuestions);

            if (quizDetail != null)
            {
                SetEntityValues(quizDetail, model);
                quizDetail.QuizQuestions = model.QuizQuestions;
                Update(quizDetail, quiz => quiz.QuizQuestions);
                return await SaveChangesAsync() > 0;
            }

            return false;
        }

        bool IQuizRepository.Insert(QuizDetail quizDetail)
        {
            Insert(quizDetail);
            return SaveChanges() > 0;
        }
    }
}