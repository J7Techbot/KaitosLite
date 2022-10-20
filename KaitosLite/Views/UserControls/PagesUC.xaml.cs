﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewLayer.ViewModels;

namespace ViewLayer.Views.UserControls
{
    /// <summary>
    /// Interaction logic for PagesUC.xaml
    /// </summary>
    public partial class PagesUC : BaseUserControl
    {
        public PagesUC()
        {
            DataContext = new PagesViewModel();
            InitializeComponent();
        }
    }
}
