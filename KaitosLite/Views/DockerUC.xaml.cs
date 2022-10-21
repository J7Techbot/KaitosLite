using System;
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
using System.Windows.Shapes;
using ViewLayer.ViewModels;

namespace ViewLayer.Views
{
    /// <summary>
    /// Interaction logic for DockerWindow.xaml
    /// </summary>
    public partial class DockerUC : UserControl
    {
        public DockerUC()
        {
            //INJECT
            DataContext = new DockerViewModel();

            InitializeComponent();
        }       
    }
}
