using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class QuizDetail
    {
        public QuizDetail()
        {
            QuizQuestions = new HashSet<QuizQuestion>();
            QuizResponses = new HashSet<QuizResponse>();
        }

        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Title { get; set; }
        public string PaidQuiz { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; }
        public virtual ICollection<QuizResponse> QuizResponses { get; set; }
    }
}