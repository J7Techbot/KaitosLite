using KaitosObjects.DTOs;
using KaitosObjects.Enums;
using Microsoft.Extensions.DependencyInjection;
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
        public async void ShowDialog<T>(object param = null) where T : Window, new()
        {
            var window = _serviceProvider.GetRequiredService<T>();

            if (window is IActivable activableWindow)
            {
                await activableWindow.ActivateAsync(param);
            }

            window.ShowDialog();

          
        }
        public void ShowDialogNjected<T>() where T : Window
        {
            //...
            //var window = serviceProvider.GetRequiredService<T>();
            //...
        }

        IServiceProvider _serviceProvider;
        UserControlManager _userControlManager;
        List<PopUpWindow> _openedPopUps;
        List<PopUpWindow> _allPopUps;
        public WindowManager(UserControlManager userControlManager, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userControlManager = userControlManager;
            _allPopUps = new List<PopUpWindow>();
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.projectComp });
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.imagesComp });
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.modsComp });
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.pagesComp });
            _allPopUps.Add(new PopUpWindow() { XKeyIdent = ComponentType.structureComp });
        }

        public void ShowPopUp(ComponentType xKey, WindowPositionDTO windowPositionDTO,BaseViewModel vm)
        {

            PopUpWindow window =_allPopUps.First(x => x.XKeyIdent == xKey);
            window.ContentControl.Content = _userControlManager.ReturnControl(xKey);
            window.DataContext = vm;
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
            _allPopUps.ForEach(x => x.ContentControl.Content = _userControlManager.ReturnControl(ComponentType.notSet));
            _allPopUps.ForEach(x => x.Hide());

        }
        public void ClearControl(ComponentType xKey)
        {
            _allPopUps.First(x=>x.XKeyIdent == xKey).ContentControl.Content = _userControlManager.ReturnControl(ComponentType.notSet);
        }
        public void RegisterEvents(ComponentType xKey, Action<object, EventArgs> onLocationChangedEvent)
        {
            PopUpWindow window = _allPopUps.First(x => x.XKeyIdent == xKey);

            window.LocationChanged += new EventHandler(onLocationChangedEvent);
            window.SizeChanged += new SizeChangedEventHandler(onLocationChangedEvent);
        }
    }
}
