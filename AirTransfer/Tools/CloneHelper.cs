using System.Text.Json;

public static class CloneHelper
{
    public static T DeepClone<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<T>(json) ?? throw new NullReferenceException($"Éî¿ËÂ¡Ê§°Ü");
    }
}
