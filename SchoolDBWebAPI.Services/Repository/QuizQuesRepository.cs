using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Repository;

namespace SchoolDBWebAPI.Services.Services
{
    public interface IQuizQuesRepository : IRepository<QuizQuestion>
    {
    }

    public class QuizQuesRepository : BaseRepository<QuizQuestion>, IQuizQuesRepository
    {
        public QuizQuesRepository(SchoolDBContext dBContext) : base(dBContext)
        {
        }
    }
}