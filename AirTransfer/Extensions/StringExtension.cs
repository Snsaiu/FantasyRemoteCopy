using System.Text;

using Newtonsoft.Json;

namespace AirTransfer.Extensions;

public static class StringExtension
{
    public static byte[] ToBytes(this string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }

    public static T ToObject<T>(this string value)
    {
        return JsonConvert.DeserializeObject<T>(value) ?? throw new InvalidCastException();
    }

    public static string ToJson(this object value)
    {
        return JsonConvert.SerializeObject(value);
    }
}