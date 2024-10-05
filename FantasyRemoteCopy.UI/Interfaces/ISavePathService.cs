using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.UI.Interfaces
{
    public interface ISavePathService
    {
        void SavePath(string path);

        string? GetPath();
    }
}
