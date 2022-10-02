using DomainLayer.DomainServices;
using DomainLayer.Managers;
using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ViewLayer.Shared;

namespace ViewLayer.ViewModels
{
    public class ModsViewModel : BaseViewModel
    {
        private ModsManager _modsManager;
        private List<ModsDTO> mods;

        public List<ModsDTO> Mods { get => mods; set { mods = value; OnPropertyChanged(); } }

        public RelayCommand AddModsCommand { get; set; }

        public ModsViewModel()
        {
            _modsManager = new ModsManager();

            AddModsCommand = new RelayCommand(param => this.OnCreateMods(), param => true);

            ProjectDomainService.SubscribeProjectChanged(ProjectChanged);
            ProjectDomainService.SubscribeSelectedProjectModsChanged(SelectedProjectModsChanged);
        }

        
        private void OnCreateMods()
        {
            if (Mods != null)
            {
                Mods.Add(_modsManager.Create());

                ProjectDomainService.InvokeSelectedProjectModsChanged(Mods);
            }
            
        }

        private void SelectedProjectModsChanged(IEnumerable<ModsDTO> Mods)
        {
            CollectionViewSource.GetDefaultView(Mods).Refresh();
        }

        private void ProjectChanged(ProjectDTO project)
        {
            if (project.Mods == null)
                project.Mods = new List<ModsDTO>();

            Mods = project.Mods;
        }        
    }
}
