using System.Text;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Extensions;

public static class StringExtension
{
    public static byte[] ToBytes(this string value) => Encoding.UTF8.GetBytes(value);

    public static T ToObject<T>(this string value) =>
        JsonConvert.DeserializeObject<T>(value) ?? throw new InvalidCastException();

    public static string ToJson(this object value) => JsonConvert.SerializeObject(value);
}