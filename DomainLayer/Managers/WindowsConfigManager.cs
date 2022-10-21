using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Managers
{
    public class WindowsConfigManager
    {
        public void SaveConfig()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
