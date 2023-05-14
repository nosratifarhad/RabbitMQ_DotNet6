using Newtonsoft.Json;
using System.Text;

namespace RabbitMQApplication.Extensions;
public static class SerializeExtension
{
    public static byte[] SerializeObjectExtension<T>(this T message)
    {
        var json = JsonConvert.SerializeObject(message);
        return Encoding.UTF8.GetBytes(json);
    }
}
