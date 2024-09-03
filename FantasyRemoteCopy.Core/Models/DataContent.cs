namespace FantasyRemoteCopy.Core.Models
{
    /// <summary>
    /// 数据详情
    /// </summary>
    public class DataContent
    {
        public string Guid { get; set; } = string.Empty;

        /// <summary>
        /// 内容，如果是文件或者图片，则是文件路径
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }
}
