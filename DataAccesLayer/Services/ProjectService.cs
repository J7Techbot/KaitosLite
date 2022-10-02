using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.Services
{
    public class ProjectService
    {
        private List<ProjectDTO> _projectDB;

        public ProjectService()
        {
            CreateDB();
        }

        private void CreateDB()
        {
            _projectDB = new List<ProjectDTO>();
            _projectDB.Add(Create("Alfa"));
            _projectDB.Add(Create("Beta"));
            _projectDB.Add(Create("Gamma"));
            _projectDB.Add(Create("Delta"));
        }
        private ProjectDTO Create(string name)
        {
            return new ProjectDTO()
            {
                Name = name,
                Marc = new MarcDTO() { Name = name+"MarcOne" },
                Mods = new List<ModsDTO>() { new ModsDTO() { Name = name + "ModsOne" }, new ModsDTO() { Name = name + "ModsTwo" }, new ModsDTO() { Name = name + "ModsThree" } },                
                Pages = new List<PageDTO>() { new PageDTO() { Name = name + "PageOne" }, new PageDTO() { Name = name + "PageTwo" }, new PageDTO() { Name = name + "PageThree" }, new PageDTO() { Name = name + "PageFour" }, new PageDTO() { Name = name + "PageFive" } }
            };
        }

        public ProjectDTO Get(string name)
        {
            return _projectDB.Where(x => x.Name == name).First();
        }
        public List<ProjectDTO> GetAll()
        {
            return _projectDB;
        }
        public void Save(ProjectDTO project)
        {
            _projectDB.Add(project);
        }
    }
}
