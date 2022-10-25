using KaitosObjects.DTOs;
using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewLayer.ViewModels;

namespace ViewLayer.Interfaces
{
    public interface IWindowManager
    {
        void ShowDialog<T>(BaseViewModel vm) where T : Window, new();
        void ShowDialogInject<T>(object param = null) where T : Window, new();
        void ShowPopUp(ComponentType xKey, WindowStatsDTO windowPositionDTO, BaseViewModel vm);
        void ClosePopUps();
        void ClearPopUpContentControl(ComponentType xKey);
        void RegisterEvents(ComponentType xKey, Action<object, EventArgs> onLocationChangedEvent);
    }
}
