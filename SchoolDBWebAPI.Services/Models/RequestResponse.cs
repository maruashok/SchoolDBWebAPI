using System.Text.Json;

namespace SchoolDBWebAPI.Services.Models
{
    public class RequestResponse
    {
        public object Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}