using KaitosObjects.DTOs;
using KaitosObjects.Enums;
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

namespace ViewLayer.Shared
{
    public class WindowManager
    {
        private Window CreateNewWindow<T>() where T : Window, new()
        {
            T newWindow = new T();
            newWindow.Show();
            return newWindow;
        }
        private Window CreateNewWindowDialog<T>() where T : Window, new()
        {
            T newWindow = new T();

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
        public void ShowDialog<T>(BaseViewModel vm) where T : Window, new()
        {
            var dialog = CreateNewWindowDialog<T>();
            dialog.DataContext = vm;
            dialog.ShowDialog();
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
        UserControlManager _userControlManager;
        List<PopUpWindow> _openedPopUps;
        List<PopUpWindow> _allPopUps;
        public WindowManager(UserControlManager userControlManager)
        {
            _userControlManager = userControlManager;
            _allPopUps = new List<PopUpWindow>();
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.projectComp });
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.imagesComp });
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.modsComp });
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.pagesComp });
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.structureComp });
        }

        public void ShowPopUp(ComponentType xKey, WindowPositionDTO windowPositionDTO)
        {

            PopUpWindow window = /*new PopUpWindow(_userControlManager.ReturnControl(xKey));*/_allPopUps.First(x => x.XKeyIdent == xKey);
            window.ContentControl.Content = _userControlManager.ReturnControl(xKey);
            
            window.Show();

            window.Left = windowPositionDTO.Left;
            window.Top = windowPositionDTO.Top;
            window.Height = windowPositionDTO.Height;
            window.Width = windowPositionDTO.Width;

            _openedPopUps ??= new List<PopUpWindow>();
            _openedPopUps.Add(window);
        }
        public void ClosePopUps()
        {
            if (_openedPopUps != null)
            {
                foreach (var popUp in _openedPopUps)
                {
                    //popUp.;
                }
                _openedPopUps.Clear();
            }            
        }
        public void ClearControl(ComponentType xKey)
        {
            _allPopUps.First(x=>x.XKeyIdent == xKey).ContentControl.Content = Application.Current.Resources["ClearControl"];
        }
        public void RegisterEvents(ComponentType xKey, Action<object, CancelEventArgs> onCloseEvent, Action<object, EventArgs> onLocationChangedEvent)
        {
            PopUpWindow window = _allPopUps.First(x => x.XKeyIdent == xKey);
            window.Closing += new CancelEventHandler(onCloseEvent);
            window.LocationChanged += new EventHandler(onLocationChangedEvent);
            window.SizeChanged += new SizeChangedEventHandler(onLocationChangedEvent);
        }
    }
}
