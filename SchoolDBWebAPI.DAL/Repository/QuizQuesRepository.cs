using SchoolDBWebAPI.DAL.DBModels;

namespace SchoolDBWebAPI.DAL.Repository
{
    public interface IQuizQuesRepository : IBaseRepository<QuizQuestion>
    {
    }

    public class QuizQuesRepository : BaseRepository<QuizQuestion>, IQuizQuesRepository
    {
        public QuizQuesRepository(SchoolDBContext dBContext) : base(dBContext)
        {
        }
    }
}