namespace AirTransfer.Interfaces;

/// <summary>
/// 可设置别名
/// </summary>
public interface IAliasable
{
    /// <summary>
    /// 别名
    /// </summary>
    string Alias { get;  }
    
}

public interface IHasModels<T>:ISelectable<T>
{
    IEnumerable<T> GetModels();
}

public interface ISelectable<T>
{
    T SelectedModel { get; set; }
}

/// <summary>
/// 显示调用
/// </summary>
public interface IHasCustomModel:IHasModels<string>
{
    /// <summary>
    /// 如果有自定义模型，则要显示调用
    /// </summary>
    /// <returns></returns>
    IEnumerable<string> GetAllModels()
    {
        var ms = ((IHasModels<string>)this).GetModels();
        return [ ..ms, "Custom"];
    }
    
    string CustomModelName { get; set; }
}

public interface IApiKey
{
    string ApiKey { get; set; }
}

public interface IApiDomain
{
    string ApiDomain { get; set; }
}

public interface IApiPath
{
    string ApiPath { get; set; }
}

public interface ITopP
{
    double TopP { get; set; }
}

// public class DemoModel:IHasCustomModel
// {
//     public IEnumerable<string> GetModels()
//     {
//         return new List<string> {"Model1", "Model2"};
//     }
//
//     public string SelectedModel { get; set; }
//     public string CustomModelName { get; }
// }





