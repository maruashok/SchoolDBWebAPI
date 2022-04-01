namespace SchoolDBWebAPI.Services.Models.SP.Query
{
    public class Qry_SP_StudentMasterSelect
    {
        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }
        public string CityId { get; set; }
        public string GenderType { get; set; }
        public int? PageNumber { get; set; }
        public int? RowsOfPage { get; set; }
    }
}