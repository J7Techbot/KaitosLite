using DomainLayer.DomainServices;
using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Managers
{
    public class GenerateModule
    {
        public void Generate()
        {
            var projects  = ProjectDomainService.ReturnSelectedProject();
            var selectedPages = ProjectDomainService.ReturnSelectedPages();
        }
    }
}
