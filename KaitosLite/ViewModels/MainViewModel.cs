using DomainLayer.DomainServices;
using DomainLayer.Managers;
using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewLayer.Shared;

namespace ViewLayer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand GenerateCommand { get; set; }
        private GenerateModule _generateModule;

        private int tabSelectedIndex;
        public int TabSelectedIndex
        {
            get => tabSelectedIndex;
            set
            {
                if (value != tabSelectedIndex)
                {
                    tabSelectedIndex = value;
                    OnPropertyChanged();

                    TabChanged(value);
                }
            }
        }

        private void TabChanged(int tab)
        {
            DockerViewModel.Init(tab);
        }

        private DockerViewModel dockerViewModel;
        public DockerViewModel DockerViewModel { get => dockerViewModel; set { dockerViewModel = value; OnPropertyChanged(); } }

        public MainViewModel(DockerViewModel dockerViewModel)
        {
            DockerViewModel = dockerViewModel;


            #region DomainService
            GenerateCommand = new RelayCommand(param => this.OnGenerate(), param => true);

            _generateModule = new GenerateModule();

            ProjectDomainService.SubscribeProjectsChanged(GetAllProjects);
            ProjectDomainService.SubscribeSelectedPagesChanged(GetSelectedPages);
            #endregion
        }




        #region DomainService
        private int projectsCount;
        private int selectedPagesCount;


        public int ProjectsCount { get => projectsCount; set { projectsCount = value; OnPropertyChanged(); } }

        public int SelectedPagesCount { get => selectedPagesCount; set { selectedPagesCount = value; OnPropertyChanged(); } }
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
        #endregion
    }
}
