using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Interfaces;
using SchoolDBWebAPI.DAL.Repository;

namespace SchoolDBWebAPI.DAL.Repository
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