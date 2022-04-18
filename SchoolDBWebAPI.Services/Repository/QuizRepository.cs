using SchoolDBWebAPI.Services.DBModels;

namespace SchoolDBWebAPI.Services.Repository
{
    public interface IQuizRepository : IRepository<QuizDetail>
    {
    }

    public class QuizRepository : BaseRepository<QuizDetail>, IQuizRepository
    {
        public QuizRepository(SchoolDBContext dBContext) : base(dBContext)
        {
        }
    }
}