﻿using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLayer.Interfaces;
using ViewLayer.Views.UserControls;

namespace ViewLayer.Shared
{
    public class UserControlManager : IUserControlManager
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
            var clearControl = new BaseUserControl();
            clearControl.XKeyIdent = ComponentType.notSet;

            _controls = new List<BaseUserControl>() { pagesUC , projectUC , modsUC , structureUC , imagesUC, clearControl };

        }

        public BaseUserControl ReturnControl(ComponentType xKey)
        {
            return _controls.First(x => x.XKeyIdent == xKey);
        }
    }
}
