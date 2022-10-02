using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Managers
{
    public class ProjectManager
    {
        public ProjectDTO Create()
        {
            return new ProjectDTO() { Name = "NewOne" };
        }
    }
}
