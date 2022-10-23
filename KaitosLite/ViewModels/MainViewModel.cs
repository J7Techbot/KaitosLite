using DomainLayer.DomainServices;
using DomainLayer.Managers;
using KaitosLite;
using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public RelayCommand ChangeThemeCommand { get; set; }
        public RelayCommand ChangeLocalizationCommand { get; set; }

        private GenerateModule _generateModule;
        private ConfigManager _configManager;

        private int tabSelectedIndex;
        public int TabSelectedIndex
        {
            get => tabSelectedIndex;
            set
            {
                Trace.WriteLine(value);
                if (tabSelectedIndex != value)
                {
                    tabSelectedIndex = value;
                    OnPropertyChanged();

                    TabChanged(value);
                }
            }
        }
       
        private DockerViewModel dockerViewModel;
        public DockerViewModel DockerViewModel { get => dockerViewModel; set { dockerViewModel = value; OnPropertyChanged(); } }


        private ResourceDictionary _localization;
        public MainViewModel(DockerViewModel dockerViewModel,ConfigManager configManager)
        {
            _localization = Application.Current.Resources;
            _configManager = configManager;
            DockerViewModel = dockerViewModel;

            ChangeThemeCommand = new RelayCommand(param => this.OnChangeTheme(), param => true);
            ChangeLocalizationCommand = new RelayCommand(param => this.OnChangeLocalization(), param => true);
            #region DomainService
            GenerateCommand = new RelayCommand(param => this.OnGenerate(), param => true);

            _generateModule = new GenerateModule();

            ProjectDomainService.SubscribeProjectsChanged(GetAllProjects);
            ProjectDomainService.SubscribeSelectedPagesChanged(GetSelectedPages);
            #endregion
        }

        private void OnChangeLocalization()
        {
            MessageBox.Show((string)_localization["ChangeLocalizationConfirmKey"]);
            var app = (App)Application.Current;
            app.ChangeLocalization(new Uri(_configManager.SwitchedLocal, UriKind.RelativeOrAbsolute));
        }

        private void OnChangeTheme()
        {
            var app = (App)Application.Current;
            app.ChangeTheme(new Uri(_configManager.SwitchedTheme, UriKind.RelativeOrAbsolute));
        }

        private void TabChanged(int tab)
        {
            DockerViewModel.Init(tab);
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
