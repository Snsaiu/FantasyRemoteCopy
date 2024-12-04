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
    }
}