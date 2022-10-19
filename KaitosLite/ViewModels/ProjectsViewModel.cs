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
            _projectManager = new ProjectManager();

            AllProjects = new ObservableCollection<ProjectDTO>(_projectManager.GetAllProjects());
            AddProjectCommand = new RelayCommand(param => this.OnProjectAdded(), param => true);
            SelectProjectCommand = new RelayCommand(param => this.OnSelectProject(param), param => true);

            ProjectDomainService.SubscribeProjectsChanged(x => AllProjects = new ObservableCollection<ProjectDTO>(x));

            ProjectDomainService.SubscribeSelectedProjectSource(() => { return _selectedProject;});
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
    }
}
