using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaitosObjects.DTOs
{
    public class ProjectDTO
    {
        public string Name { get; set; }
        public MarcDTO Marc { get; set; }
        public List<PageDTO> Pages { get; set; }
        public List<ModsDTO> Mods { get; set; }
    }
}
