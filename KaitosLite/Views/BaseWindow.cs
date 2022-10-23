using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewLayer.Views
{
    public class BaseWindow : Window
    {
        public static readonly DependencyProperty XKeyIdentProperty = DependencyProperty.Register("XKeyIdent", typeof(ComponentType), typeof(BaseWindow));

        public ComponentType XKeyIdent
        {
            get => (ComponentType)GetValue(XKeyIdentProperty);
            set => SetValue(XKeyIdentProperty, value);
        }
    }
}
