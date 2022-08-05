using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.DAL.DBModels
{
    public partial class QuizResponse
    {
        public QuizResponse()
        {
            QueResponses = new HashSet<QueResponse>();
        }

        public int Id { get; set; }
        public int QuizId { get; set; }
        public int StudentId { get; set; }
        public DateTime? SubmitDate { get; set; }
        public decimal? QuizScore { get; set; }

        public virtual QuizDetail Quiz { get; set; }
        public virtual ICollection<QueResponse> QueResponses { get; set; }
    }
}
