using Newtonsoft.Json;
using SchoolDBWebAPI.Services.DBModels;
using System.IO;

namespace SchoolDBWebAPI.Services.Test
{
    public class FeedDBData
    {
        public static QuizDetail GetQuizDetail()
        {
            QuizDetail quizDetail = default;

            string JsonData = File.ReadAllText(@"JsonFiles\QuizController\AddQuiz.txt");

            if (!string.IsNullOrEmpty(JsonData))
            {
                quizDetail = JsonConvert.DeserializeObject<QuizDetail>(JsonData);
            }

            return quizDetail;
        }
    }
}