using Newtonsoft.Json;
using SchoolDBWebAPI.DAL.DBModels;
using System.Collections.Generic;
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

        public static List<QuizDetail> GetQuizDetails()
        {
            List<QuizDetail> quizDetails = default;

            string JsonData = File.ReadAllText(@"JsonFiles\QuizController\Quizes.txt");

            if (!string.IsNullOrEmpty(JsonData))
            {
                quizDetails = JsonConvert.DeserializeObject<List<QuizDetail>>(JsonData);
            }

            return quizDetails;
        }
    }
}