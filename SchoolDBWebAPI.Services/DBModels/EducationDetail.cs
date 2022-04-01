using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.Services.DBModels
{
    public partial class EducationDetail
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int SemesterId { get; set; }
        public int BatchId { get; set; }
        public int YearJoined { get; set; }

        public virtual EducationCouse Course { get; set; }
        public virtual StudentMaster Student { get; set; }
    }
}
