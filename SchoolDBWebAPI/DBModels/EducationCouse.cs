using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class EducationCouse
    {
        public EducationCouse()
        {
            EducationDetails = new HashSet<EducationDetail>();
        }

        public int Id { get; set; }
        public string CourseName { get; set; }
        public int NoOfSemesters { get; set; }

        public virtual ICollection<EducationDetail> EducationDetails { get; set; }
    }
}