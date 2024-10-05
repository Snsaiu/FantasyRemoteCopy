using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.UI.Interfaces.Impls
{
    public class SavePathService : ISavePathService
    {
        public string? GetPath() => Preferences.Default.Get<string>("savepath", string.Empty);

        public void SavePath(string path) => Preferences.Default.Set<string>("savePath", path);
    }
}
