using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Interfaces;

namespace SchoolDBWebAPI.DAL.Repository
{
    public class QuizRepository : Repository<QuizDetail>, IQuizRepository
    {
        private readonly ILogger logger;

        public QuizRepository(ILogger<QuizRepository> _logger, SchoolDBContext dBContext) : base(dBContext)
        {
            logger = _logger;
        }

        //public QuizDetail AddQuiz(List<DBSQLParameter> SQLParams)
        //{
        //    return ExecStoreProcedure<QuizDetail>("SP_QuizDetailInsert", SQLParams)?.FirstOrDefault();
        //}

        public bool DeleteByID(int QuizId)
        {
            DeleteById(QuizId);
            return SaveChanges() > 0;
        }

        //public List<QuizDetail> GetAll(Qry_SP_StudentMasterSelect model)
        //{
        //    return ExecStoreProcedure<QuizDetail>("SP_StudentMasterSelect", model);
        //}

        public bool IsQuizExists(int QuizId)
        {
            bool IsExists = false;

            try
            {
                IsExists = Query(quiz => quiz.Id == QuizId).FirstOrDefault() != null;
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex, Ex.Message);
            }

            return IsExists;
        }

        //public QuizDetail QuizWithQues(int QuizId)
        //{
        //    var paramList = new List<DBSQLParameter>();
        //    paramList.Add(new DBSQLParameter("@QuizId", QuizId));
        //    var result = ExecStoreProcedureList<QuizDetail, QuizQuestion>("SP_QuizWithQues", paramList);
        //    return (result.Item1.Map(result.Item2, quiz => quiz.Id, quizQues => quizQues.QuizId, (quiz, ques) => { quiz.QuizQuestions = ques.ToList(); }) as List<QuizDetail>).FirstOrDefault();
        //}

        public async Task<QuizDetail> QuizWithQuesAsync(int QuizId)
        {
            return await Query(quiz => quiz.Id == QuizId).Include(q => q.QuizQuestions).FirstAsync();
        }

        //public List<QuizDetail> SearchQuizByTitle(string QuizTitle)
        //{
        //    return GetWithRawSql($@"Select * from QuizDetail where Title like '%' + @Qry +'%'", QuizTitle).ToList();
        //}

        public async Task<bool> UpdateAsync(QuizDetail model)
        {
            QuizDetail quizDetail = await Query(quiz => quiz.Id == model.Id).FirstOrDefaultAsync();

            if (quizDetail != null)
            {
                Update(quizDetail);
                return await SaveChangesAsync() > 0;
            }

            return false;
        }
    }
}