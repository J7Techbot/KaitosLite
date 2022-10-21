using DomainLayer.Managers;
using GongSolutions.Wpf.DragDrop;
using KaitosObjects.DTOs;
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

namespace ViewLayer.ViewModels
{
    public class DockerViewModel : BaseViewModel
    {
        private bool isColumn1Visible;
        public bool IsColumn1Visible { get => isColumn1Visible; set { isColumn1Visible = value; OnPropertyChanged(); } }

        private bool isColumn2Visible;
        public bool IsColumn2Visible { get => isColumn2Visible; set { isColumn2Visible = value; OnPropertyChanged(); } }

        private bool isColumn3Visible;
        public bool IsColumn3Visible { get => isColumn3Visible; set { isColumn3Visible = value; OnPropertyChanged(); } }

        private string _xKey1;
        private string _xKey2;
        private string _xKey3;

        public RelayCommand PopUpModuleCommand { get; set; }
        public RelayCommand ModuleOrderCommand { get; set; }
        public RelayCommand SaveOrderCommand { get; set; }


        private ModuleOrderDTO[] _modules;
        public ObservableCollection<ModuleOrderDTO> ModulesOrderCollection { get; set; }

        WindowsConfigManager _configManager;
        WindowManager _windowManager;
        DispatcherTimer _resizeTimer;
        PopUpWindow _movedWindow;

        public DockerViewModel()
        {
            _windowManager = new WindowManager();

            _resizeTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1500), IsEnabled = false };
            _resizeTimer.Tick += SavePositionToConf;

            PopUpModuleCommand = new RelayCommand(param => this.OnPopUpOpen(param), param => true);
            ModuleOrderCommand = new RelayCommand(param => this.OnModuleOrder(), param => true);
            SaveOrderCommand = new RelayCommand(param => this.OnSaveOrder(), param => true);


            //this is collection of all modules and must be loaded from Db, or file
            _modules = new ModuleOrderDTO[] { new ModuleOrderDTO() { Name = "Projekty", XKey = "ProjectUC" },
                                              new ModuleOrderDTO() { Name = "Mods", XKey = "ModsUC" },
                                              new ModuleOrderDTO() { Name = "Stránky", XKey = "PagesUC" }};


            #region LOAD FROM CONFIG

            //set modules for this viewModel
            _xKey1 = _modules[0].XKey; //index number or select key to collection must be defined by outside,config?
            _xKey2 = _modules[1].XKey;
            _xKey3 = _modules[2].XKey;

            ModulesOrderCollection = new ObservableCollection<ModuleOrderDTO>();

            Application.Current.Resources["Module1"] = Application.Current.Resources[_xKey1];
            ModulesOrderCollection.Add(_modules.First(x => x.XKey == _xKey1));

            Application.Current.Resources["Module2"] = Application.Current.Resources[_xKey2];
            ModulesOrderCollection.Add(_modules.First(x => x.XKey == _xKey2));

            Application.Current.Resources["Module3"] = Application.Current.Resources[_xKey3];
            ModulesOrderCollection.Add(_modules.First(x => x.XKey == _xKey3));


            IsColumn1Visible = true;
            IsColumn2Visible = true;
            IsColumn3Visible = true;

            #endregion
        }

        private void OnSaveOrder()
        {
            bool free1 = true, free2 = true;
            foreach (var module in ModulesOrderCollection)
            {
                if (IsColumn1Visible && free1)
                {
                    free1 = false;
                    _xKey1 = module.XKey;
                    Application.Current.Resources["Module1"] = Application.Current.Resources[module.XKey];
                }
                else if (IsColumn2Visible && free2)
                {
                    free2 = false;
                    _xKey2 = module.XKey;
                    Application.Current.Resources["Module2"] = Application.Current.Resources[module.XKey];
                }
                else
                {
                    _xKey3 = module.XKey;
                    Application.Current.Resources["Module3"] = Application.Current.Resources[module.XKey];
                }
            }

            //save to config
        }

        private void OnModuleOrder()
        {
            _windowManager.Show<ModuleOrderWindow>(this);
        }
       
        private void OnPopUpOpen(object param)
        {
            int collectionKey = int.Parse((string)param);
            string xKey = "";
            switch (collectionKey)
            {
                case 0:
                    xKey = _xKey1;
                    if (isColumn2Visible)
                    {
                        if (isColumn3Visible)
                        {
                            _xKey1 = _xKey2;
                            _xKey2 = _xKey3;

                            IsColumn3Visible = false;

                            Application.Current.Resources["Module1"] = Application.Current.Resources[_xKey1];
                            Application.Current.Resources["Module2"] = Application.Current.Resources[_xKey2];
                        }
                        else
                        {
                            _xKey1 = _xKey2;

                            IsColumn2Visible = false;

                            Application.Current.Resources["Module1"] = Application.Current.Resources[_xKey1];
                        }
                    }
                    break;
                case 1:
                    xKey = _xKey2;
                    if (isColumn3Visible)
                    {
                        _xKey2 = _xKey3;

                        IsColumn3Visible = false;

                        Application.Current.Resources["Module2"] = Application.Current.Resources[_xKey2];
                    }
                    else
                    {

                        IsColumn2Visible = false;
                        Application.Current.Resources["Module2"] = Application.Current.Resources["ClearControl"];
                    }
                    break;
                case 2:
                    xKey = _xKey3;
                    IsColumn3Visible = false;
                    Application.Current.Resources["Module3"] = Application.Current.Resources["ClearControl"];
                    break;
            }

            var userControl = (BaseUserControl)Application.Current.Resources[xKey];
            userControl.XKeyIdent = xKey;

            _windowManager.ShowPopUp(userControl, PopUpClose, PopUpSizeLocationChanged);
           
            ModulesOrderCollection.Remove(ModulesOrderCollection.First(x => x.XKey == xKey));
        }

        private void PopUpSizeLocationChanged(object sender, EventArgs e)
        {
            _movedWindow = (PopUpWindow)sender;
            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        void SavePositionToConf(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;

            //TODO:save position/size to config
            var popUpXKey = ((BaseUserControl)_movedWindow.ContentControl.Content).XKeyIdent;
            var h = _movedWindow.Height;
            var w = _movedWindow.Width;
            var t = _movedWindow.Top;
            var l = _movedWindow.Left;
        }

        private void PopUpClose(object sender, CancelEventArgs e)
        {
            BaseUserControl uc = (BaseUserControl)(sender as PopUpWindow).ContentControl.Content;

            if (!IsColumn2Visible)
            {
                _xKey2 = uc.XKeyIdent;
                IsColumn2Visible = true;
                Application.Current.Resources["Module2"] = Application.Current.Resources[uc.XKeyIdent];
            }
            else if (!IsColumn3Visible)
            {
                _xKey3 = uc.XKeyIdent;
                IsColumn3Visible = true;
                Application.Current.Resources["Module3"] = Application.Current.Resources[uc.XKeyIdent];
            }

            SortOrderCollection();
        }

        private void SortOrderCollection()
        {
            ModulesOrderCollection.Clear();
            if (IsColumn1Visible)
                ModulesOrderCollection.Add(_modules.First(x => x.XKey == _xKey1));
            if (IsColumn2Visible)
                ModulesOrderCollection.Add(_modules.First(x => x.XKey == _xKey2));
            if (IsColumn3Visible)
                ModulesOrderCollection.Add(_modules.First(x => x.XKey == _xKey3));
        }
    }
}
