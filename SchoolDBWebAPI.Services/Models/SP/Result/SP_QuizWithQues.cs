using System;
using System.Collections.Generic;

namespace SchoolDBWebAPI.Services.Models.SP.Result
{
    public class SP_QuizWithQues
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public string PaidQuiz { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<SP_QuizWithQues_Ques> SP_QuizWithQues_Ques { get; set; }
    }

    public class SP_QuizWithQues_Ques
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Description { get; set; }
    }
}