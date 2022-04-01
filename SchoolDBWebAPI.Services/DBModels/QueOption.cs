using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.Services.DBModels
{
    public partial class QueOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string OptionValue { get; set; }
        public int IsCorrect { get; set; }

        public virtual QuizQuestion Question { get; set; }
    }
}
