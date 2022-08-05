using System;

namespace SchoolDBWebAPI.DAL.Models.SP.Quiz
{
    public class SP_QuizDetailInsert
    {
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PaidQuiz { get; set; }
    }
}