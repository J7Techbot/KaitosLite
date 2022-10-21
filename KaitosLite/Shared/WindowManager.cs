using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ViewLayer.ViewModels;
using ViewLayer.Views;
using ViewLayer.Views.UserControls;

namespace ViewLayer.Managers
{
    public class WindowManager
    {
        private Window CreateNewWindow<T>() where T : Window,new()
        {
            T newWindow = new T();            
            newWindow.Show();
            return newWindow;
        }
        public void Show<T>() where T : Window, new()
        {
            CreateNewWindow<T>();
        }
        public void Show<T>(BaseViewModel vm) where T : Window, new()
        {
            CreateNewWindow<T>().DataContext = vm;            
        }
        public void ShowNjected<T>() where T : Window
        {
            //...
            //var window = serviceProvider.GetRequiredService<T>();
            //...
        }
        public void ShowDialog<T>() where T : Window, new()
        {
            T newWindow = new T();
            newWindow.ShowDialog();
        }
        public void ShowDialogNjected<T>() where T : Window
        {
            //...
            //var window = serviceProvider.GetRequiredService<T>();
            //...
        }
        public void ShowPopUp(BaseUserControl userControl, Action<object, CancelEventArgs> onCloseEvent, Action<object, EventArgs> onLocationChangedEvent)
        { 
            PopUpWindow newWindow = new PopUpWindow(userControl);
            newWindow.Closing += new CancelEventHandler(onCloseEvent);
            newWindow.LocationChanged += new EventHandler(onLocationChangedEvent);
            newWindow.SizeChanged += new SizeChangedEventHandler(onLocationChangedEvent);
            newWindow.Show();

        }
    }
}
