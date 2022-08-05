using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.DAL.DBModels
{
    public partial class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
