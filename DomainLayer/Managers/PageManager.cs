﻿using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Managers
{
    public class PageManager
    {
        public PageDTO Create()
        {
            return new PageDTO() { Name = "NewOne" };
        }
    }
}
