using System.Text.Json;

namespace Entities.Exceptions;

public class ErrorModel
{
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
