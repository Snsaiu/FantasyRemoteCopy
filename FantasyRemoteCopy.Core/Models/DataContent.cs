using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.Core.Models
{
    /// <summary>
    /// 数据详情
    /// </summary>
    public class DataContent
    {
        public string Guid { get; set; }

        /// <summary>
        /// 内容，如果是文件或者图片，则是文件路径
        /// </summary>
        public string Content { get; set; }
    }
}
