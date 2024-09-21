using System.Text;

namespace FantasyRemoteCopy.UI.Extensions;

public static class StringExtension
{
    public static byte[] ToBytes(this string value)=>Encoding.UTF8.GetBytes(value);
}