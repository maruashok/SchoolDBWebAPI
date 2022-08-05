using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolDBWebAPI.DAL.DBModels
{
    public partial class Country
    {
        public int Id { get; set; }
        public string Sortname { get; set; }
        public string Name { get; set; }
        public int Phonecode { get; set; }
    }
}
