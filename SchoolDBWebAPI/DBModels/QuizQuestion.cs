using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class QuizQuestion
    {
        public QuizQuestion()
        {
            QueOptions = new HashSet<QueOption>();
        }

        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Description { get; set; }

        public virtual QuizDetail Quiz { get; set; }
        public virtual ICollection<QueOption> QueOptions { get; set; }
    }
}