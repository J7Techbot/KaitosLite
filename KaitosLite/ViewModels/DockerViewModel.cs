using GongSolutions.Wpf.DragDrop;
using KaitosObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ViewLayer.Shared;
using ViewLayer.Views;

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

        private bool disableOrderButtons;
        public bool DisableOrderButtons { get => disableOrderButtons; set { disableOrderButtons = value; OnPropertyChanged(); } }

        public RelayCommand PopUpModuleCommand { get; set; }
        public RelayCommand PopUpDownModuleCommand { get; set; }

        public RelayCommand ModuleOrderCommand { get; set; }
        public RelayCommand SaveOrderCommand { get; set; }

        public ObservableCollection<ModuleOrderDTO> ModulesOrderCollection { get; set; }

        public DockerViewModel()
        {
            DisableOrderButtons = true;

            PopUpModuleCommand = new RelayCommand(param => this.OnPopUp(param), param => true);
            PopUpDownModuleCommand = new RelayCommand(param => this.OnPopDown(param), param => true);
            ModuleOrderCommand = new RelayCommand(param => this.OnModuleOrder(), param => true);
            SaveOrderCommand = new RelayCommand(param => this.OnSaveOrder(), param => true);

            //load from config
            Application.Current.Resources["ProjectsModule1"] = Application.Current.Resources["ProjectUC"];
            Application.Current.Resources["ProjectsModule2"] = Application.Current.Resources["ModsUC"];
            Application.Current.Resources["ProjectsModule3"] = Application.Current.Resources["PagesUC"];

            ModulesOrderCollection = new ObservableCollection<ModuleOrderDTO>();
            ModulesOrderCollection.Add(new ModuleOrderDTO() { Name = "Projekty", XKey = "ProjectUC" });
            ModulesOrderCollection.Add(new ModuleOrderDTO() { Name = "Mods", XKey = "ModsUC" });
            ModulesOrderCollection.Add(new ModuleOrderDTO() { Name = "Stránky", XKey = "PagesUC" });

            IsColumn1Visible = true;
            IsColumn2Visible = true;
            IsColumn3Visible = true;

            
        }

        private void OnSaveOrder()
        {
            if (ModulesOrderCollection.Count > 0 && IsColumn1Visible)
                Application.Current.Resources["ProjectsModule1"] = Application.Current.Resources[ModulesOrderCollection[0].XKey];
            if (ModulesOrderCollection.Count > 1 && IsColumn2Visible)
                Application.Current.Resources["ProjectsModule2"] = Application.Current.Resources[ModulesOrderCollection[1].XKey];
            if (ModulesOrderCollection.Count > 2 && IsColumn3Visible)
                Application.Current.Resources["ProjectsModule3"] = Application.Current.Resources[ModulesOrderCollection[2].XKey];

            //save to config
        }

        private void OnModuleOrder()
        {
            ModuleOrderWindow moduleOrderWindow = new ModuleOrderWindow(this);
            moduleOrderWindow.Show();
        }

        private void OnPopDown(object param)
        {
            Application.Current.Resources["t"] = Application.Current.Resources["e"];
        }

        private void OnPopUp(object param)
        {
            int collectionKey = int.Parse((string)param);

            switch (collectionKey)
            {

                case 0:
                    IsColumn1Visible = false;
                    Application.Current.Resources["ProjectsModule1"] = Application.Current.Resources["ClearControl"];
                    break;
                case 1:
                    IsColumn2Visible = false;
                    Application.Current.Resources["ProjectsModule2"] = Application.Current.Resources["ClearControl"];

                    break;
                case 2:
                    IsColumn3Visible = false;
                    Application.Current.Resources["ProjectsModule3"] = Application.Current.Resources["ClearControl"];

                    break;
            }

            var uc = (UserControl)Application.Current.Resources[ModulesOrderCollection[collectionKey].XKey];
            PopUp1Window popUp1Window = new PopUp1Window(uc);
            popUp1Window.Show();

            ModulesOrderCollection.RemoveAt(collectionKey);
        }
    }
}
