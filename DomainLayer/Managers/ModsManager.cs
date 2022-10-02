using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Managers
{
    public class ModsManager
    {
        public ModsDTO Create()
        {
            return new ModsDTO() { Name = "NewOne" };
        }
    }
}
