using System.Text;
using System.Text.Json;

namespace Microservice.Order.History.Function.Helpers;

public class JsonHelper
{
    public static T GetRequest<T>(byte[] message)
    {
        return message == null
            ? throw new ArgumentNullException("Message parameter cannot be null.")
            : JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(message));
    }
}
