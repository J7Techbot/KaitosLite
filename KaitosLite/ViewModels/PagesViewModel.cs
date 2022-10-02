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
    public class PagesViewModel : BaseViewModel
    {
        private List<PageDTO> pages;
        public List<PageDTO> Pages { get => pages; set { pages = value; OnPropertyChanged(); } }

        public RelayCommand SelectPageCommand { get; set; }
        public RelayCommand AddPageCommand { get; set; }

        private IEnumerable<PageDTO> _selectedPages;

        private PageManager _pageManager;

        public PagesViewModel()
        {
            ProjectDomainService.SubscribeProjectChanged(ProjectChanged);
            ProjectDomainService.SubscribeToSelectedPagesSource(ReturnSelectedPages);
            ProjectDomainService.SubscribeProjectPagesChanged(ProjectPagesChanged);

            SelectPageCommand = new RelayCommand(param => this.OnSelectionChanged(param), param => true);
            AddPageCommand = new RelayCommand(param => this.OnAddPage(), param => true);

            _pageManager = new PageManager();
        }

        private void OnAddPage()
        {
            if (Pages != null)
            {
                Pages.Add(_pageManager.Create());
                ProjectDomainService.InvokeProjectPagesChanged(Pages);
            }            
        }
        private void ProjectPagesChanged(IEnumerable<PageDTO> pages)
        {
            CollectionViewSource.GetDefaultView(pages).Refresh();
        }
        private void ProjectChanged(ProjectDTO project)
        {
            if (project.Pages == null)
                project.Pages = new List<PageDTO>();

            Pages = project.Pages;
               
        }
        private IEnumerable<PageDTO> ReturnSelectedPages()
        {
            return _selectedPages;
        }
        public void OnSelectionChanged(object param)
        {
            IEnumerable<object> collection = param as IEnumerable<object>;
            var selectedPages = new List<PageDTO>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    selectedPages.Add((PageDTO)item);
                }
                
                _selectedPages = selectedPages;

                ProjectDomainService.InvokeSelectedPagesChanged(_selectedPages);
            }           
        }
    }
}
