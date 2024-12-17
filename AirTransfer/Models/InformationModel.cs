using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirTransfer.Enums;

namespace AirTransfer.Models
{
    public class InformationModel
    {

        public SendType SendType { get; set; }

        public string? Text { get; set; }

        public string? FolderPath { get; set; }

        public IEnumerable<string>? Files { get; set; }

        public string? Summary => SendType switch
        {
            SendType.Text => Text?[..Math.Min(Text.Length, 20)],
            SendType.Folder => FolderPath?[..Math.Min(FolderPath.Length, 20)],
            SendType.File => string.Join(",", Files ?? throw new ArgumentNullException())[..Math.Min(string.Join(",", Files).Length, 20)],
            _ => throw new NotImplementedException()
        };
    }
}