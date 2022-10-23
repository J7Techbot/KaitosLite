using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLayer.Views.UserControls;

namespace ViewLayer.Shared
{
    public class UserControlManager
    {
        PagesUC pagesUC;
        ProjectUC projectUC;
        ModsUC modsUC;
        StructureUC structureUC;
        ImagesUC imagesUC;

        List<BaseUserControl> _controls;

        public UserControlManager(PagesUC pagesUC, ProjectUC projectUC, ModsUC modsUC, StructureUC structureUC, ImagesUC imagesUC)
        {
            this.pagesUC = pagesUC;
            this.pagesUC.XKeyIdent = ComponentType.pagesComp;
            this.projectUC = projectUC;
            this.projectUC.XKeyIdent = ComponentType.projectComp;
            this.modsUC = modsUC;
            this.modsUC.XKeyIdent = ComponentType.modsComp;
            this.structureUC = structureUC;
            this.structureUC.XKeyIdent = ComponentType.structureComp;
            this.imagesUC = imagesUC;
            this.imagesUC.XKeyIdent = ComponentType.imagesComp;

            _controls = new List<BaseUserControl>() { pagesUC , projectUC , modsUC , structureUC , imagesUC };

        }

        public BaseUserControl ReturnControl(ComponentType xKey)
        {
            return _controls.First(x => x.XKeyIdent == xKey);
        }
    }
}
