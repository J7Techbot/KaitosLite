using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ViewLayer.Views.UserControls
{
    public class BaseUserControl : UserControl
    {
        public static readonly DependencyProperty XKeyIdentProperty = DependencyProperty.Register("XKeyIdent", typeof(ComponentType), typeof(BaseUserControl));

        public ComponentType XKeyIdent
        {
            get => (ComponentType)GetValue(XKeyIdentProperty);
            set => SetValue(XKeyIdentProperty, value);
        }

        public static readonly DependencyProperty ReservedColumnProperty = DependencyProperty.Register("ReservedColumn", typeof(int), typeof(BaseUserControl));

        public int ReservedColumn
        {
            get => (int)GetValue(ReservedColumnProperty);
            set => SetValue(ReservedColumnProperty, value);
        }
    }
}
