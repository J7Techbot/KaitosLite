using DomainLayer.Managers;
using GongSolutions.Wpf.DragDrop;
using KaitosObjects.DTOs;
using KaitosObjects.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ViewLayer.Managers;
using ViewLayer.Shared;
using ViewLayer.Views;
using ViewLayer.Views.UserControls;
using static DomainLayer.Managers.WindowsConfigManager;

namespace ViewLayer.ViewModels
{
    public class DockerViewModel : BaseViewModel
    {
        string column0Width;
        public string Column0Width { get => column0Width; set { column0Width = value; OnPropertyChanged(); } }
        string column1Width;
        public string Column1Width { get => column1Width; set { column1Width = value; OnPropertyChanged(); } }
        string column2Width;
        public string Column2Width { get => column2Width; set { column2Width = value; OnPropertyChanged(); } }
        

        private bool isColumn0Visible;
        public bool IsColumn0Visible { get => isColumn0Visible; set { isColumn0Visible = value; OnPropertyChanged(); } }

        private bool isColumn1Visible;
        public bool IsColumn1Visible { get => isColumn1Visible; set { isColumn1Visible = value; OnPropertyChanged(); } }

        private bool isColumn2Visible;
        public bool IsColumn2Visible { get => isColumn2Visible; set { isColumn2Visible = value; OnPropertyChanged(); } }

        private ComponentType _xKey0;
        private ComponentType _xKey1;
        private ComponentType _xKey2;

        public RelayCommand PopUpModuleCommand { get; set; }
        public RelayCommand ModuleOrderCommand { get; set; }
        public RelayCommand SaveOrderCommand { get; set; }
        public RelayCommand SplitterDragEndCommand { get; set; }


        private ComponentsOrderDTO[] _components;
        public ObservableCollection<ComponentsOrderDTO> ComponentsOrderCollection { get; set; }
        

        ComponentsManager _moduleManager;
        WindowsConfigManager _configManager;
        WindowManager _windowManager;
        SModule _moduleSettings;
        DispatcherTimer _resizeTimer;
        PopUpWindow _movedWindow;

        public DockerViewModel(ComponentsManager moduleManager, WindowManager windowManager, WindowsConfigManager windowsConfigManager)
        {
            _windowManager = windowManager;
            _moduleManager = moduleManager;
            _configManager = windowsConfigManager;
            _components = _moduleManager.GetModules().ToArray();

            _resizeTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1500), IsEnabled = false };
            _resizeTimer.Tick += PopUpSaveToConf;

            PopUpModuleCommand = new RelayCommand(param => this.OnPopUpOpen(param), param => true);
            ModuleOrderCommand = new RelayCommand(param => this.OnOpenModuleOrder(), param => true);
            SaveOrderCommand = new RelayCommand(param => this.OnSaveModuleOrder(), param => true);
            SplitterDragEndCommand = new RelayCommand(param => this.OnSplitterDragEnd(), param => true);

            Init();
        }        

        public void Init(int tabModule = 0)
        {
            Application.Current.Resources["Component0"] = Application.Current.Resources["ClearControl"];
            Application.Current.Resources["Component1"] = Application.Current.Resources["ClearControl"];
            Application.Current.Resources["Component2"] = Application.Current.Resources["ClearControl"];

            ComponentsOrderCollection = new ObservableCollection<ComponentsOrderDTO>();

            #region LOAD FROM CONFIG

            _moduleSettings = _configManager.LoadFromConfig(tabModule);
            if (_moduleSettings != null)
            {
                _xKey0 = _configManager.ReturnXKey(_moduleSettings, order: 0);
                if (_xKey0 != ComponentType.notSet)
                {
                    IsColumn0Visible = true;
                    Application.Current.Resources["Component0"] = Application.Current.Resources[_xKey0.ToString()];
                    ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey0));

                    Column0Width = _configManager.ReturnValue(_moduleSettings, _xKey0, "width").ToString();
                    if ((bool)_configManager.ReturnValue(_moduleSettings, _xKey0, "detached"))
                        OnPopUpOpen("0");
                }

                _xKey1 = _configManager.ReturnXKey(_moduleSettings, order: 1);
                if (_xKey1 != ComponentType.notSet)
                {
                    IsColumn1Visible = true;
                    Application.Current.Resources["Component1"] = Application.Current.Resources[_xKey1.ToString()];
                    ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey1));

                    Column1Width = _configManager.ReturnValue(_moduleSettings, _xKey1, "width").ToString();
                    if ((bool)_configManager.ReturnValue(_moduleSettings, _xKey1, "detached"))
                        OnPopUpOpen("1");
                }
                else
                    IsColumn1Visible = false;

                _xKey2 = _configManager.ReturnXKey(_moduleSettings, order: 2);
                if (_xKey2 != ComponentType.notSet)
                {
                    IsColumn2Visible = true;
                    Application.Current.Resources["Component2"] = Application.Current.Resources[_xKey2.ToString()];
                    ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey2));

                    Column2Width = _configManager.ReturnValue(_moduleSettings, _xKey2, "width").ToString();                   
                    if ((bool)_configManager.ReturnValue(_moduleSettings, _xKey2, "detached"))
                        OnPopUpOpen("2");
                }
                else
                    IsColumn2Visible = false;
            }
            

            #endregion
        }
        private void OnOpenModuleOrder()
        {
            _windowManager.Show<ModuleOrderWindow>(this);
        }
        private void OnSaveModuleOrder()
        {
            bool free1 = true, free2 = true;
            foreach (var module in ComponentsOrderCollection)
            {
                if (IsColumn0Visible && free1)
                {
                    free1 = false;
                    _xKey0 = module.XKey;
                    Application.Current.Resources["Component0"] = Application.Current.Resources[module.XKey.ToString()];

                   _configManager.UpdateSettings(_moduleSettings, _xKey0, "order", 0);
                }
                else if (IsColumn1Visible && free2)
                {
                    free2 = false;
                    _xKey1 = module.XKey;
                    Application.Current.Resources["Component1"] = Application.Current.Resources[module.XKey.ToString()];

                    _configManager.UpdateSettings(_moduleSettings, _xKey1,"order", 1);
                }
                else
                {
                    _xKey2 = module.XKey;
                    Application.Current.Resources["Component2"] = Application.Current.Resources[module.XKey.ToString()];

                    _configManager.UpdateSettings(_moduleSettings, _xKey2, "order", 2);
                }
            }

            //save to config
            _configManager.SaveToConfig(_moduleSettings);
        }              

        private void OnPopUpOpen(object param)
        {
            int collectionKey = int.Parse((string)param);
            ComponentType xKey = ComponentType.projectComp;
            switch (collectionKey)
            {
                case 0:
                    xKey = _xKey0;
                    if (isColumn1Visible)
                    {
                        if (isColumn2Visible)
                        {
                            _xKey0 = _xKey1;
                            _xKey1 = _xKey2;

                            IsColumn2Visible = false;

                            Application.Current.Resources["Component0"] = Application.Current.Resources[_xKey0.ToString()];
                            Application.Current.Resources["Component1"] = Application.Current.Resources[_xKey1.ToString()];
                        }
                        else
                        {
                            _xKey0 = _xKey1;

                            IsColumn1Visible = false;

                            Application.Current.Resources["Component0"] = Application.Current.Resources[_xKey0.ToString()];
                        }
                    }
                    break;
                case 1:
                    xKey = _xKey1;
                    if (isColumn2Visible)
                    {
                        _xKey1 = _xKey2;

                        IsColumn2Visible = false;

                        Application.Current.Resources["Component1"] = Application.Current.Resources[_xKey1.ToString()];
                    }
                    else
                    {

                        IsColumn1Visible = false;
                        Application.Current.Resources["Component1"] = Application.Current.Resources["ClearControl"];
                    }
                    break;
                case 2:
                    xKey = _xKey2;
                    IsColumn2Visible = false;
                    Application.Current.Resources["Component2"] = Application.Current.Resources["ClearControl"];
                    break;
            }

            PopUp(xKey);

            _configManager.UpdateSettings(_moduleSettings, xKey, "detached", true);

            _configManager.SaveToConfig(_moduleSettings);
        }
        private void PopUp(ComponentType xKey)
        {
            var userControl = (BaseUserControl)Application.Current.Resources[xKey.ToString()];
            userControl.XKeyIdent = xKey;

            _windowManager.ShowPopUp(userControl, OnPopUpClose, PopUpSizeLocationChanged);

            ComponentsOrderCollection.Remove(ComponentsOrderCollection.First(x => x.XKey == xKey));
        }
        private void OnPopUpClose(object sender, CancelEventArgs e)
        {
            BaseUserControl uc = (BaseUserControl)(sender as PopUpWindow).ContentControl.Content;

            if (!IsColumn1Visible)
            {
                _xKey1 = uc.XKeyIdent;
                IsColumn1Visible = true;
                Application.Current.Resources["Component1"] = Application.Current.Resources[uc.XKeyIdent.ToString()];

                _configManager.UpdateSettings(_moduleSettings, _xKey1, "detached", false);                
            }
            else if (!IsColumn2Visible)
            {
                _xKey2 = uc.XKeyIdent;
                IsColumn2Visible = true;
                Application.Current.Resources["Component2"] = Application.Current.Resources[uc.XKeyIdent.ToString()];

                _configManager.UpdateSettings(_moduleSettings, _xKey2, "detached", false);
            }

            SortModuleOrderCollection();

            _configManager.SaveToConfig(_moduleSettings);
        }
        private void PopUpSizeLocationChanged(object sender, EventArgs e)
        {
            _movedWindow = (PopUpWindow)sender;
            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }
        private void PopUpSaveToConf(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;

            //TODO:save position/size to config
            var xKey = ((BaseUserControl)_movedWindow.ContentControl.Content).XKeyIdent;
            var h = _movedWindow.Height;
            var w = _movedWindow.Width;
            var t = _movedWindow.Top;
            var l = _movedWindow.Left;

            _configManager.UpdateSettings(_moduleSettings, xKey, "height", h, detached: true) ;
            _configManager.UpdateSettings(_moduleSettings, xKey, "width", w, detached: true);
            _configManager.UpdateSettings(_moduleSettings, xKey, "Y", t, detached: true);
            _configManager.UpdateSettings(_moduleSettings, xKey, "X", l, detached: true);

            _configManager.SaveToConfig(_moduleSettings);
        }

        private void OnSplitterDragEnd()
        {
            _configManager.UpdateSettings(_moduleSettings, _xKey0, "width", Column0Width);
            _configManager.UpdateSettings(_moduleSettings, _xKey1, "width", Column1Width);
            _configManager.UpdateSettings(_moduleSettings, _xKey2, "width", Column2Width);
            _configManager.SaveToConfig(_moduleSettings);
        }
        private void SortModuleOrderCollection()
        {
            ComponentsOrderCollection.Clear();
            if (IsColumn0Visible)
                ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey0));
            if (IsColumn1Visible)
                ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey1));
            if (IsColumn2Visible)
                ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey2));
        }

    }
}
