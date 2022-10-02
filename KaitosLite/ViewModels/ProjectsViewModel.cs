using DomainLayer.DomainServices;
using DomainLayer.Managers;
using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLayer.Shared;

namespace ViewLayer.ViewModels
{
    public class ProjectsViewModel : BaseViewModel
    {
        private ProjectManager _projectManager;

        private ObservableCollection<ProjectDTO> allProjects;
        public ObservableCollection<ProjectDTO> AllProjects
        {
            get => allProjects;
            set
            {
                allProjects = value;
                OnPropertyChanged();               
            }
        }

        private ProjectDTO _selectedProject;
        public RelayCommand AddProjectCommand { get; set; }
        public RelayCommand SelectProjectCommand { get; set; }

        public ProjectsViewModel()
        {
            AllProjects = new ObservableCollection<ProjectDTO>(ProjectDomainService.GetAllProjects());
            AddProjectCommand = new RelayCommand(param => this.OnProjectAdded(), param => true);
            SelectProjectCommand = new RelayCommand(param => this.OnSelectProject(param), param => true);

            _projectManager = new ProjectManager();

            ProjectDomainService.SubscribeProjectsChanged(ProjectsChanged);
            ProjectDomainService.SubscribeToSelectedProjectSource(ReturnSelectedProject);
        }

        private void OnSelectProject(object param)
        {
            _selectedProject = (ProjectDTO)param;
            ProjectDomainService.InvokeProjectChanged((ProjectDTO)param);
        }

        private void OnProjectAdded()
        {
            AllProjects.Add(_projectManager.Create());

            ProjectDomainService.InvokeAllProjectsChanged(AllProjects);
        }
        private void ProjectsChanged(IEnumerable<ProjectDTO> projects)
        {
            AllProjects = new ObservableCollection<ProjectDTO>( projects);
        }
        private ProjectDTO ReturnSelectedProject()
        {
            return _selectedProject;
        }
    }
}
