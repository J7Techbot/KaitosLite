using DomainLayer.Interfaces;
using KaitosObjects.DTOs;
using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DomainLayer.Managers
{
    public class ComponentManager : IComponentManager
    {
        public void SetComponent(string compName,object value)
        {
            Application.Current.Resources[compName] = value;
        }
        public object GetComponent(string compName)
        {
            return Application.Current.Resources[compName];
        }
        public IEnumerable<ComponentsOrderDTO> GetComponentTypes()
        {
            //this is collection of all modules and must be loaded from Db, or file
            return new ComponentsOrderDTO[] { new ComponentsOrderDTO() { Name = "Projekty", XKey = ComponentType.projectComp },
                                              new ComponentsOrderDTO() { Name = "Mods", XKey = ComponentType.modsComp },
                                              new ComponentsOrderDTO() { Name = "Stránky", XKey = ComponentType.pagesComp },
                                              new ComponentsOrderDTO() { Name = "Struktura", XKey = ComponentType.structureComp },
                                              new ComponentsOrderDTO() { Name = "Obrázky", XKey = ComponentType.imagesComp } };
        }
    }
}
