#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class QueResponse
    {
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public int? CorrectOptId { get; set; }
        public int? IsCorrectAns { get; set; }
        public decimal? QueScore { get; set; }

        public virtual QuizResponse Response { get; set; }
    }
}