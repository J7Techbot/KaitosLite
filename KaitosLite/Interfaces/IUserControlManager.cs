using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLayer.Views.UserControls;

namespace ViewLayer.Interfaces
{
    public interface IUserControlManager
    {
        BaseUserControl ReturnControl(ComponentType xKey);        
    }
}
