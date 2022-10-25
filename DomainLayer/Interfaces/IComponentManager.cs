using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface IComponentManager
    {
        void SetComponent(string compName, object value);

        object GetComponent(string compName);

        IEnumerable<ComponentsOrderDTO> GetComponentTypes();
    }
}
