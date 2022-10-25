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
using System.Windows.Media;
using System.Windows.Threading;
using ViewLayer.Shared;
using ViewLayer.Views;
using ViewLayer.Views.UserControls;
using static DomainLayer.Managers.ConfigManager;

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

        private ComponentType _componentXKey0;
        private ComponentType _componentXKey1;
        private ComponentType _componentXKey2;

        public RelayCommand PopUpOpenCommand { get; set; }
        public RelayCommand PopUpCloseCommand { get; set; }
        public RelayCommand ModuleOrderCommand { get; set; }
        public RelayCommand SaveOrderCommand { get; set; }
        public RelayCommand SplitterDragEndCommand { get; set; }


        private ComponentsOrderDTO[] _components;
        public ObservableCollection<ComponentsOrderDTO> ComponentsOrderCollection { get; set; }

        UserControlManager _userControlManager;
        ComponentsManager _moduleManager;
        ConfigManager _configManager;
        WindowManager _windowManager;
        SModule _moduleSettings;
        DispatcherTimer _resizeTimer;
        PopUpWindow _movedWindow;

        private int _moduleNumber;
        public DockerViewModel(ComponentsManager moduleManager, WindowManager windowManager, ConfigManager windowsConfigManager, UserControlManager userControlManager)
        {
            _windowManager = windowManager;
            _moduleManager = moduleManager;
            _configManager = windowsConfigManager;
            _userControlManager = userControlManager;
            _components = _moduleManager.GetModules().ToArray();

            _resizeTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500), IsEnabled = false };
            _resizeTimer.Tick += PopUpSaveToConf;

            PopUpOpenCommand = new RelayCommand(param => this.OnPopUpOpen(param), param => true);
            PopUpCloseCommand = new RelayCommand(param => this.OnPopUpClose(param), param => true);
            ModuleOrderCommand = new RelayCommand(param => this.OnOpenModuleOrder(), param => true);
            SaveOrderCommand = new RelayCommand(param => this.OnSaveModuleOrder(), param => true);
            SplitterDragEndCommand = new RelayCommand(param => this.OnSplitterDragEnd(), param => true);

            

            Init();
        }
        
        public void Init(int moduleNumber = 0)
        {
            _moduleNumber = moduleNumber;            
            _windowManager.ClosePopUps();
            _moduleSettings = _configManager.LoadFromConfig(moduleNumber);


            ComponentsOrderCollection = InitComponentOrderCollection(_moduleSettings);

            ClearPanels();

            RegisterEvents(ComponentsOrderCollection);

            InitAtachedComponents(InitColumns(InitXKeys(_moduleSettings)));

            InitDetachedComponents(_moduleSettings);

            void RegisterEvents(IEnumerable<ComponentsOrderDTO> compsInModule)
            {
                foreach (var comp in compsInModule)
                {
                    _windowManager.RegisterEvents(comp.XKey, PopUpSizeLocationChanged);
                }                
            }
            void ClearPanels()
            {
                Application.Current.Resources["Component0"] = _userControlManager.ReturnControl(ComponentType.notSet);
                Application.Current.Resources["Component1"] = _userControlManager.ReturnControl(ComponentType.notSet);
                Application.Current.Resources["Component2"] = _userControlManager.ReturnControl(ComponentType.notSet);
            }            
            int InitXKeys(SModule moduleSettings)
            {
                _componentXKey0 = ComponentType.notSet;
                _componentXKey1 = ComponentType.notSet;
                _componentXKey2 = ComponentType.notSet;

                var attachedPanels = moduleSettings.Panels.Where(x => !x.detached).OrderBy(x => x.order).ToList();

                _componentXKey0 = _configManager.ReturnXKey(moduleSettings, attachedPanels[0].order);               

                if (attachedPanels.Count > 1)
                    _componentXKey1 = _configManager.ReturnXKey(moduleSettings, attachedPanels[1].order);

                if (attachedPanels.Count > 2)
                    _componentXKey2 = _configManager.ReturnXKey(moduleSettings, attachedPanels[2].order);

                return attachedPanels.Count;

            }
            int InitColumns(int countOfAttachedComponents)
            {
                IsColumn0Visible = false;
                IsColumn1Visible = false;
                IsColumn2Visible = false;

                switch (countOfAttachedComponents)
                {
                    case 1:
                        IsColumn0Visible = true;

                        Column0Width = _configManager.ReturnValue(_moduleSettings, _componentXKey0, "width").ToString();
                        break;

                    case 2:
                        IsColumn0Visible = true;
                        IsColumn1Visible = true;

                        Column0Width = _configManager.ReturnValue(_moduleSettings, _componentXKey0, "width").ToString();
                        Column1Width = _configManager.ReturnValue(_moduleSettings, _componentXKey1, "width").ToString();
                        break;

                    case 3:
                        IsColumn0Visible = true;
                        IsColumn1Visible = true;
                        IsColumn2Visible = true;

                        Column0Width = _configManager.ReturnValue(_moduleSettings, _componentXKey0, "width").ToString();
                        Column1Width = _configManager.ReturnValue(_moduleSettings, _componentXKey1, "width").ToString();
                        Column2Width = _configManager.ReturnValue(_moduleSettings, _componentXKey2, "width").ToString();
                        break;
                }

                return countOfAttachedComponents;
            }
            void InitAtachedComponents(int countOfAttachedComponents)
            {
                Application.Current.Resources["Component0"] = _userControlManager.ReturnControl(_componentXKey0);

                if(countOfAttachedComponents > 1)
                    Application.Current.Resources["Component1"] = _userControlManager.ReturnControl(_componentXKey1);
                if (countOfAttachedComponents > 2)
                    Application.Current.Resources["Component2"] = _userControlManager.ReturnControl(_componentXKey2);
            }
            void InitDetachedComponents(SModule moduleSettings)
            {
                var dettachedPanels = moduleSettings.Panels.Where(x => x.detached).OrderBy(x => x.order).ToList();

                foreach (var item in dettachedPanels)
                {
                    ShowPopUp(_configManager.ReturnXKey(moduleSettings, item.order));
                }
            }
            ObservableCollection<ComponentsOrderDTO> InitComponentOrderCollection(SModule moduleSettings)
            {
                ObservableCollection<ComponentsOrderDTO> retCollection = new ObservableCollection<ComponentsOrderDTO>();
                var comps = moduleSettings.Panels.OrderBy(x => x.order).Select(x => x.component);
                foreach (var comp in comps)
                {
                    retCollection.Add(_components.First(x => x.XKey == (ComponentType)Enum.Parse(typeof(ComponentType), comp)));
                }

                return retCollection;
            }

            //bool? IsComponentDetached(ComponentType xKey)
            //{
            //    bool? xKeyDetached = null;
            //    if (xKey != ComponentType.notSet)
            //    {
            //        if ((bool)_configManager.ReturnValue(_moduleSettings, xKey, "detached"))
            //            xKeyDetached = true;
            //        else
            //            xKeyDetached = false;
            //    }

            //    return xKeyDetached;
            //}
            //_moduleSettings = _configManager.LoadFromConfig(tabModule);
            //if (_moduleSettings != null)
            //{
            //    _xKey0 = _configManager.ReturnXKey(_moduleSettings, order: 0);
            //    if (_xKey0 != ComponentType.notSet)
            //    {
            //        IsColumn0Visible = true;
            //        Application.Current.Resources["Component0"] = _userControlManager.ReturnControl(_xKey0);
            //        ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey0));

            //        Column0Width = _configManager.ReturnValue(_moduleSettings, _xKey0, "width").ToString();                       

            //        _windowManager.RegisterEvents(_xKey0, PopUpSizeLocationChanged);
            //    }

            //    _xKey1 = _configManager.ReturnXKey(_moduleSettings, order: 1);
            //    if (_xKey1 != ComponentType.notSet)
            //    {
            //        IsColumn1Visible = true;
            //        Application.Current.Resources["Component1"] = _userControlManager.ReturnControl(_xKey1);
            //        ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey1));

            //        Column1Width = _configManager.ReturnValue(_moduleSettings, _xKey1, "width").ToString();
            //        if ((bool)_configManager.ReturnValue(_moduleSettings, _xKey1, "detached"))
            //            OnPopUpOpen("1");

            //        _windowManager.RegisterEvents(_xKey1, PopUpSizeLocationChanged);
            //    }
            //    else
            //    {
            //        IsColumn1Visible = false;
            //    }


            //    _xKey2 = _configManager.ReturnXKey(_moduleSettings, order: 2);
            //    if (_xKey2 != ComponentType.notSet)
            //    {
            //        IsColumn2Visible = true;
            //        Application.Current.Resources["Component2"] = _userControlManager.ReturnControl(_xKey2);
            //        ComponentsOrderCollection.Add(_components.First(x => x.XKey == _xKey2));

            //        Column2Width = _configManager.ReturnValue(_moduleSettings, _xKey2, "width").ToString();
            //        if ((bool)_configManager.ReturnValue(_moduleSettings, _xKey2, "detached"))
            //            OnPopUpOpen("2");

            //        _windowManager.RegisterEvents(_xKey2, PopUpSizeLocationChanged);
            //    }
            //    else
            //    {
            //        IsColumn2Visible = false;
            //    }

            //}


        }
       
        private void ShowPopUp(ComponentType xKey)
        {
            _windowManager.ShowPopUp(xKey, _configManager.ReturnWindowPosition(_moduleSettings, xKey), this);
        }

        private void OnPopUpOpen(object param)
        {
            ComponentType xKey = ComponentType.notSet;
            if (param is string)
            {
                var columnNumber = (string)param;
                switch (columnNumber)
                {
                    case "0":
                        xKey = ((BaseUserControl)Application.Current.Resources["Component0"]).XKeyIdent;
                        break;
                    case "1":
                        xKey = ((BaseUserControl)Application.Current.Resources["Component1"]).XKeyIdent;
                        break;
                    case "2":
                        xKey = ((BaseUserControl)Application.Current.Resources["Component2"]).XKeyIdent;
                        break;
                }
            }


            _configManager.UpdateSettings(_moduleSettings, xKey, "detached", true);
            _configManager.SaveToConfig(_moduleSettings);

            Init(_moduleNumber);
            
            
            //int collectionKey = int.Parse((string)param);
            //ComponentType xKey = ComponentType.notSet;
            //bool clearOne = false;
            //switch (collectionKey)
            //{
            //    //detach column 0
            //    case 0:
            //        xKey = _componentXKey0;

            //        if (isColumn1Visible)
            //        {

            //            if (isColumn2Visible)
            //            {
            //                IsColumn2Visible = false;

            //                _componentXKey0 = _componentXKey1;
            //                _componentXKey1 = _componentXKey2;
            //                _componentXKey2 = xKey;
            //            }
            //            else
            //            {
            //                IsColumn1Visible = false;

            //                _componentXKey0 = _componentXKey1;

            //                if (_componentXKey2 == ComponentType.notSet)                            
            //                    _componentXKey1 = xKey;
            //                else
            //                {
            //                    _componentXKey1 = _componentXKey2;
            //                    _componentXKey2 = xKey;
            //                }


            //                clearOne = true;
            //            }
            //        }
            //        break;
            //    //detach column 1
            //    case 1:
            //        xKey = _componentXKey1;

            //        if (isColumn2Visible)
            //        {
            //            IsColumn2Visible = false;

            //            _componentXKey1 = _componentXKey2;
            //            _componentXKey2 = xKey;
            //        }
            //        else
            //        {
            //            IsColumn1Visible = false;

            //            clearOne = true;
            //        }
            //        break;

            //    //detach column 2
            //    case 2:
            //        xKey = _componentXKey2;

            //        IsColumn2Visible = false;

            //        break;
            //}

            //Application.Current.Resources["Component0"] = _userControlManager.ReturnControl(_componentXKey0);
            //Application.Current.Resources["Component1"] = _userControlManager.ReturnControl(clearOne ? ComponentType.notSet : _componentXKey1);
            //Application.Current.Resources["Component2"] = _userControlManager.ReturnControl(ComponentType.notSet);

            //_configManager.UpdateSettings(_moduleSettings, _componentXKey0, "order", 0);
            //_configManager.UpdateSettings(_moduleSettings, _componentXKey1, "order", 1);
            //_configManager.UpdateSettings(_moduleSettings, _componentXKey2, "order", 2);
            //_configManager.UpdateSettings(_moduleSettings, xKey, "detached", true);
            //_configManager.SaveToConfig(_moduleSettings);

            //_windowManager.ShowPopUp(xKey, _configManager.ReturnWindowPosition(_moduleSettings, xKey), this);

            //ComponentsOrderCollection.Remove(ComponentsOrderCollection.First(x => x.XKey == xKey));
        }
        private void OnPopUpClose(object sender)
        {
            var window = sender as PopUpWindow;
            var xKey = window.XKeyIdent;

            _windowManager.ClearControl(xKey);

            _configManager.UpdateSettings(_moduleSettings, xKey, "detached", value: false);

            _configManager.SaveToConfig(_moduleSettings);

            Init(_moduleNumber);

            //if (!IsColumn1Visible)
            //{
            //    _componentXKey1 = xKey;

            //    IsColumn1Visible = true;

            //    Application.Current.Resources["Component1"] = _userControlManager.ReturnControl(xKey);

            //    _configManager.UpdateSettings(_moduleSettings, _componentXKey1, "detached", value: false);
            //    _configManager.UpdateSettings(_moduleSettings, _componentXKey1, "order", value: 1);

            //    var lastDetached = _moduleSettings.Panels.FirstOrDefault(x => x.detached)?.component;
            //    if (lastDetached != null)
            //        _configManager.UpdateSettings(_moduleSettings, (ComponentType)Enum.Parse(typeof(ComponentType), lastDetached), "order", value: 2);

            //    _windowManager.ClearControl(xKey);
            //}
            //else if (!IsColumn2Visible)
            //{
            //    _componentXKey2 = xKey;

            //    IsColumn2Visible = true;

            //    Application.Current.Resources["Component2"] = _userControlManager.ReturnControl(xKey);

            //    _configManager.UpdateSettings(_moduleSettings, _componentXKey2, "detached", value: false);
            //    _configManager.UpdateSettings(_moduleSettings, _componentXKey2, "order", value: 2);

            //    _windowManager.ClearControl(xKey);
            //}

            //SortModuleOrderCollection();

            //_configManager.SaveToConfig(_moduleSettings);

            //window.Visibility = Visibility.Collapsed;

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
            double h = _movedWindow.Height;
            double w = _movedWindow.Width;
            double t = _movedWindow.Top;
            double l = _movedWindow.Left;

            _configManager.UpdateSettings(_moduleSettings, xKey, "height", h, detachedWindowProp: true);
            _configManager.UpdateSettings(_moduleSettings, xKey, "width", w, detachedWindowProp: true);
            _configManager.UpdateSettings(_moduleSettings, xKey, "Y", t, detachedWindowProp: true);
            _configManager.UpdateSettings(_moduleSettings, xKey, "X", l, detachedWindowProp: true);

            _configManager.SaveToConfig(_moduleSettings);
        }

        private void OnSplitterDragEnd()
        {
            _configManager.UpdateSettings(_moduleSettings, _componentXKey0, "width", Column0Width);
            _configManager.UpdateSettings(_moduleSettings, _componentXKey1, "width", Column1Width);
            _configManager.UpdateSettings(_moduleSettings, _componentXKey2, "width", Column2Width);
            _configManager.SaveToConfig(_moduleSettings);
        }

        private void OnOpenModuleOrder()
        {
            _windowManager.ShowDialog<ModuleOrderWindow>(this);
        }
        private void OnSaveModuleOrder()
        {
            _configManager.UpdateSettings(_moduleSettings, ComponentsOrderCollection.ToArray()[0].XKey, "order", 0);
            _configManager.UpdateSettings(_moduleSettings, ComponentsOrderCollection.ToArray()[1].XKey, "order", 1);
            _configManager.UpdateSettings(_moduleSettings, ComponentsOrderCollection.ToArray()[2].XKey, "order", 2);

            //bool free1 = true, free2 = true;
            //foreach (var module in ComponentsOrderCollection)
            //{
            //    if (IsColumn0Visible && free1)
            //    {
            //        free1 = false;
            //        _componentXKey0 = module.XKey;
            //        Application.Current.Resources["Component0"] = _userControlManager.ReturnControl(module.XKey);

            //        _configManager.UpdateSettings(_moduleSettings, _componentXKey0, "order", 0);
            //    }
            //    else if (IsColumn1Visible && free2)
            //    {
            //        free2 = false;
            //        _componentXKey1 = module.XKey;
            //        Application.Current.Resources["Component1"] = _userControlManager.ReturnControl(module.XKey);

            //        _configManager.UpdateSettings(_moduleSettings, _componentXKey1, "order", 1);
            //    }
            //    else
            //    {
            //        _componentXKey2 = module.XKey;
            //        Application.Current.Resources["Component2"] = _userControlManager.ReturnControl(module.XKey);

            //        _configManager.UpdateSettings(_moduleSettings, _componentXKey2, "order", 2);
            //    }
            //}

            //save to config
            _configManager.SaveToConfig(_moduleSettings);

            Init(_moduleNumber);
        }

        private void SortModuleOrderCollection()
        {
            ComponentsOrderCollection.Clear();
            if (IsColumn0Visible && _componentXKey0 != ComponentType.notSet)
                ComponentsOrderCollection.Add(_components.First(x => x.XKey == _componentXKey0));
            if (IsColumn1Visible && _componentXKey1 != ComponentType.notSet)
                ComponentsOrderCollection.Add(_components.First(x => x.XKey == _componentXKey1));
            if (IsColumn2Visible && _componentXKey1 != ComponentType.notSet)
                ComponentsOrderCollection.Add(_components.First(x => x.XKey == _componentXKey2));
        }

    }
}
