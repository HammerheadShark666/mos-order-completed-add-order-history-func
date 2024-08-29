using System.Text;
using System.Text.Json;

namespace Microservice.Order.History.Function.Helpers;

public class JsonHelper
{
    public static T? GetRequest<T>(byte[] message)
    {
        ArgumentNullException.ThrowIfNull(message);
        return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(message));
    }
}