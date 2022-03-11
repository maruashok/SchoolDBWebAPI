#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class Country
    {
        public int Id { get; set; }
        public string Sortname { get; set; }
        public string Name { get; set; }
        public int Phonecode { get; set; }
    }
}