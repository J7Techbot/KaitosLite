using DomainLayer.DomainServices;
using DomainLayer.Managers;
using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLayer.Shared;

namespace ViewLayer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand GenerateCommand { get; set; }
        private GenerateModule _generateModule;

        private int projectsCount;
        private int selectedPagesCount;

        public int ProjectsCount { get => projectsCount; set { projectsCount = value; OnPropertyChanged(); } }

        public int SelectedPagesCount { get => selectedPagesCount; set { selectedPagesCount = value;OnPropertyChanged(); } }

        public MainViewModel()
        {
            GenerateCommand = new RelayCommand(param => this.OnGenerate(), param => true);

            _generateModule = new GenerateModule();

            ProjectDomainService.SubscribeProjectsChanged(GetAllProjects);
            ProjectDomainService.SubscribeSelectedPagesChanged(GetSelectedPages);
        }

        private void OnGenerate()
        {
            _generateModule.Generate();
        }

        private void GetAllProjects(IEnumerable<ProjectDTO> projects)
        {
            ProjectsCount = projects.Count();
        }
        private void GetSelectedPages(IEnumerable<PageDTO> pages)
        {
            SelectedPagesCount = pages.Count();
        }
    }
}
