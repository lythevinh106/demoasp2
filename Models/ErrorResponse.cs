using System.Text.Json;

namespace demoAsp2.Models
{
    public class ErrorResponse
    {

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
