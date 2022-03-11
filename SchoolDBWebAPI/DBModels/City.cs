#nullable disable

namespace SchoolDBWebAPI.DBModels
{
    public partial class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
    }
}