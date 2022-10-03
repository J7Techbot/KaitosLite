using DataAccesLayer.Services;
using DomainLayer.DomainServices;
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
        //Debug prop DB
        private IEnumerable<ProjectDTO> _projects;

        public IEnumerable<ProjectDTO> GetAllProjects()
        {
            //DAL service layer call
            if (_projects != null)
            {
                ProjectDomainService.InvokeAllProjectsChanged(_projects);
                return _projects;
            }

            else
            {
                _projects = new ProjectService().GetAll();
                ProjectDomainService.InvokeAllProjectsChanged(_projects);
                return _projects;
            }
        }
        public ProjectDTO Create()
        {
            return new ProjectDTO() { Name = "NewOne" };
        }
    }
}
