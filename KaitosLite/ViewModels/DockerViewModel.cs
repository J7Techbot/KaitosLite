using DomainLayer.Interfaces;
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
using ViewLayer.Extensions;
using ViewLayer.Interfaces;
using ViewLayer.Shared;
using ViewLayer.Views;
using ViewLayer.Views.UserControls;
using static DomainLayer.Managers.ConfigManager;

namespace ViewLayer.ViewModels
{
    public class DockerViewModel : BaseViewModel
    {
        private string column0Width;
        public string Column0Width { get => column0Width; set { column0Width = value; OnPropertyChanged(); } }

        private string column1Width;
        public string Column1Width { get => column1Width; set { column1Width = value; OnPropertyChanged(); } }

        private string column2Width;
        public string Column2Width { get => column2Width; set { column2Width = value; OnPropertyChanged(); } }


        private bool isColumn0Visible;
        public bool IsColumn0Visible { get => isColumn0Visible; set { isColumn0Visible = value; OnPropertyChanged(); } }

        private bool isColumn1Visible;
        public bool IsColumn1Visible { get => isColumn1Visible; set { isColumn1Visible = value; OnPropertyChanged(); } }

        private bool isColumn2Visible;
        public bool IsColumn2Visible { get => isColumn2Visible; set { isColumn2Visible = value; OnPropertyChanged(); } }

        public ObservableCollection<ComponentsOrderDTO> ComponentsOrderCollection { get; set; }
        
        private ComponentType _componentXKey0;
        private ComponentType _componentXKey1;
        private ComponentType _componentXKey2;

        private int _moduleNumber;

        public RelayCommand PopUpOpenCommand { get; set; }
        public RelayCommand PopUpCloseCommand { get; set; }
        public RelayCommand ModuleOrderCommand { get; set; }
        public RelayCommand SaveOrderCommand { get; set; }
        public RelayCommand SplitterDragEndCommand { get; set; }        

        IUserControlManager _userControlManager;
        IComponentManager _componentManager;
        IConfigManager _configManager;
        IWindowManager _windowManager;
        SModule _moduleSettings;
        DispatcherTimer _resizeTimer;
        PopUpWindow _movedWindow;
        
        public DockerViewModel(IComponentManager moduleManager, IWindowManager windowManager, IConfigManager windowsConfigManager, IUserControlManager userControlManager)
        {
            ComponentsOrderCollection = new ObservableCollection<ComponentsOrderDTO>();

            _windowManager = windowManager;
            _componentManager = moduleManager;
            _configManager = windowsConfigManager;
            _userControlManager = userControlManager;

            _resizeTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500), IsEnabled = false };
            _resizeTimer.Tick += PopUpSaveStats;

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

            InitPanels();

            InitComponentOrderCollection(_moduleSettings);
            
            InitPopUpEvents(ComponentsOrderCollection);

            InitAtachedComponents(InitColumns(InitXKeys(_moduleSettings)));

            InitDetachedComponents(_moduleSettings);


            void InitPopUpEvents(IEnumerable<ComponentsOrderDTO> compsInModule)
            {
                foreach (var comp in compsInModule)
                {
                    _windowManager.RegisterEvents(comp.XKey, PopUpSizeLocationChanged);
                }                
            }

            void InitPanels()
            {
                _componentManager.SetComponent("Component0", _userControlManager.ReturnControl(ComponentType.notSet));
                _componentManager.SetComponent("Component1", _userControlManager.ReturnControl(ComponentType.notSet));
                _componentManager.SetComponent("Component2", _userControlManager.ReturnControl(ComponentType.notSet));
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
                _componentManager.SetComponent("Component0", _userControlManager.ReturnControl(_componentXKey0));

                if(countOfAttachedComponents > 1)
                    _componentManager.SetComponent("Component1",_userControlManager.ReturnControl(_componentXKey1));
                if (countOfAttachedComponents > 2)
                    _componentManager.SetComponent("Component2",_userControlManager.ReturnControl(_componentXKey2));
            }

            void InitDetachedComponents(SModule moduleSettings)
            {
                var dettachedPanels = moduleSettings.Panels.Where(x => x.detached).OrderBy(x => x.order).ToList();

                foreach (var item in dettachedPanels)
                {
                    ShowPopUp(_configManager.ReturnXKey(moduleSettings, item.order));
                }
            }

            void InitComponentOrderCollection(SModule moduleSettings)
            {
                ComponentsOrderCollection.Clear();

                var selectedComps = _configManager.ReturnXKeys(moduleSettings);
                var allComps = _componentManager.GetComponentTypes();

                foreach (var comp in selectedComps)
                {
                    ComponentsOrderCollection.Add(allComps.First(x => x.XKey == comp));
                }
            }
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
                        xKey = ((BaseUserControl)_componentManager.GetComponent("Component0")).XKeyIdent;
                        break;
                    case "1":
                        xKey = ((BaseUserControl)_componentManager.GetComponent("Component1")).XKeyIdent;
                        break;
                    case "2":
                        xKey = ((BaseUserControl)_componentManager.GetComponent("Component2")).XKeyIdent;
                        break;
                }
            }

            _configManager.UpdateSettings(_moduleSettings, xKey, "detached", true);
            _configManager.SaveToConfig(_moduleSettings);

            Init(_moduleNumber);

        }
        private void OnPopUpClose(object sender)
        {
            var window = sender as PopUpWindow;
            var xKey = window.XKeyIdent;

            _windowManager.ClearPopUpContentControl(xKey);
            _configManager.UpdateSettings(_moduleSettings, xKey, "detached", value: false);
            _configManager.SaveToConfig(_moduleSettings);

            Init(_moduleNumber);
        }
        private void ShowPopUp(ComponentType xKey)
        {
            _windowManager.ShowPopUp(xKey, _configManager.ReturnWindowPosition(_moduleSettings, xKey), this);
        }       
        private void PopUpSizeLocationChanged(object sender, EventArgs e)
        {
            _movedWindow = (PopUpWindow)sender;
            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }
        private void PopUpSaveStats(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;

            var xKey = ((BaseUserControl)_movedWindow.ContentControl.Content).XKeyIdent;
            var windowStats = _movedWindow.GetSizeAndPosition();           

            _configManager.UpdateSettings(_moduleSettings, xKey, "height", windowStats.Height, detachedWindowProp: true);
            _configManager.UpdateSettings(_moduleSettings, xKey, "width", windowStats.Width, detachedWindowProp: true);
            _configManager.UpdateSettings(_moduleSettings, xKey, "Y", windowStats.Top, detachedWindowProp: true);
            _configManager.UpdateSettings(_moduleSettings, xKey, "X", windowStats.Left, detachedWindowProp: true);
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
            _configManager.SaveToConfig(_moduleSettings);

            Init(_moduleNumber);
        }

        private void OnSplitterDragEnd()
        {
            _configManager.UpdateSettings(_moduleSettings, _componentXKey0, "width", Column0Width);
            _configManager.UpdateSettings(_moduleSettings, _componentXKey1, "width", Column1Width);
            _configManager.UpdateSettings(_moduleSettings, _componentXKey2, "width", Column2Width);
            _configManager.SaveToConfig(_moduleSettings);
        }       
    }
}
