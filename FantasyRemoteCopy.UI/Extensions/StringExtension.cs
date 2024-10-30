using System.Text;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Extensions;

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
}