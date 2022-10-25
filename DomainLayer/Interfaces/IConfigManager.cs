using KaitosObjects.DTOs;
using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Managers.ConfigManager;

namespace DomainLayer.Interfaces
{
    public interface IConfigManager
    {
        string SwitchedLocal { get;}
        string SwitchedTheme { get;}
        SModule LoadFromConfig(int moduleType);

        void SaveToConfig(SModule module);

        ComponentType ReturnXKey(SModule module, int order);
        ComponentType[] ReturnXKeys(SModule module);

        void UpdateSettings(SModule module, ComponentType _xKey, string propertyName, object value, bool detachedWindowProp = false);

        object ReturnValue(SModule module, ComponentType _xKey, string propertyName, bool detached = false);

        WindowStatsDTO ReturnWindowPosition(SModule module, ComponentType xKey);
    }
}
