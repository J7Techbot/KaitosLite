using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLayer.Interfaces
{
    public interface ILocalizationManager
    {
        string GetByKey(string key, string[] paras = null);
    }
}
