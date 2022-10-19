using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccesLayer.Services;
using KaitosObjects.DTOs;

namespace DomainLayer.DomainServices
{
    public static class ProjectDomainService
    {        
        private static Func<ProjectDTO> SelectProject;
        private static Func<IEnumerable<PageDTO>> SelectPages;

        //GET SELECTED PROJECT
        //ONLY ONE reference is allowed
        public static void SubscribeSelectedProjectSource(Func<ProjectDTO> action)
        {           
            SelectProject = action;
        }
        public static ProjectDTO ReturnSelectedProject() => SelectProject.Invoke();

        //GET SELECTED PAGES
        //ONLY ONE reference is allowed
        public static void SubscribeSelectedPagesSource(Func<IEnumerable<PageDTO>> action)
        {           
            SelectPages = action;
        }         
        public static IEnumerable<PageDTO> ReturnSelectedPages() => SelectPages.Invoke();
 

        //SELECTED PROJECT CHANGED
        private static Action<ProjectDTO> SelectedProjectChangedEvent;
        public static void SubscribeProjectChanged(Action<ProjectDTO> action)
        {
            SelectedProjectChangedEvent += action;
        }
        public static void InvokeProjectChanged(ProjectDTO project)
        {
            SelectedProjectChangedEvent.Invoke(project);
        }

        //ALL PROJECTS CHANGED
        private static Action<IEnumerable<ProjectDTO>> ProjectsChangedEvent;
        public static void SubscribeProjectsChanged(Action<IEnumerable<ProjectDTO>> action)
        {
            ProjectsChangedEvent += action;
        }
        public static void InvokeAllProjectsChanged(IEnumerable<ProjectDTO> projects)
        {
            if(ProjectsChangedEvent != null)
                ProjectsChangedEvent.Invoke(projects);
        }

        //SELECTED PAGES CHANGED
        private static Action<IEnumerable<PageDTO>> SelectedPagesChangedEvent;
        public static void SubscribeSelectedPagesChanged(Action<IEnumerable<PageDTO>> action)
        {
            SelectedPagesChangedEvent += action;
        }
        public static void InvokeSelectedPagesChanged(IEnumerable<PageDTO> pages)
        {
            if (SelectedPagesChangedEvent != null)
                SelectedPagesChangedEvent.Invoke(pages);
        }

        //Project PAGES CHANGED
        private static Action<IEnumerable<PageDTO>> ProjectPagesChangedEvent;
        public static void SubscribeProjectPagesChanged(Action<IEnumerable<PageDTO>> action)
        {
            ProjectPagesChangedEvent += action;
        }
        public static void InvokeProjectPagesChanged(IEnumerable<PageDTO> pages)
        {
            if (ProjectPagesChangedEvent != null)
                ProjectPagesChangedEvent.Invoke(pages);
        }

        //SELECTED PROJECT MODS CHANGED
        private static Action<IEnumerable<ModsDTO>> SelectedProjectModsChangedEvent;
        public static void SubscribeSelectedProjectModsChanged(Action<IEnumerable<ModsDTO>> action)
        {
            SelectedProjectModsChangedEvent += action;
        }
        public static void InvokeSelectedProjectModsChanged(IEnumerable<ModsDTO> pages)
        {
            if (SelectedProjectModsChangedEvent != null)
                SelectedProjectModsChangedEvent.Invoke(pages);
        }
    }
}
