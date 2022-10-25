
using DomainLayer.Interfaces;
using DomainLayer.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewLayer.Interfaces;
using ViewLayer.Shared;
using ViewLayer.ViewModels;
using ViewLayer.Views;
using ViewLayer.Views.UserControls;

namespace KaitosLite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            this.serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            // Managers
            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IComponentManager, ComponentManager>();
            services.AddSingleton<IUserControlManager, UserControlManager>();
            services.AddSingleton<IConfigManager, ConfigManager>();
            services.AddSingleton<ILocalizationManager, LocalizationManager>();
            services.AddSingleton<ScanManager>();
            services.AddSingleton<PageManager>();
            services.AddSingleton<ModsManager>();
            services.AddSingleton<ProjectManager>();
            

            //ViewModels
            services.AddSingleton<DockerViewModel>();
            services.AddSingleton<MainViewModel>();

            //Windows
            services.AddSingleton<MainWindow>();

            //User controls
            
            services.AddSingleton<ProjectUC>();
            services.AddSingleton<PagesUC>();
            services.AddSingleton<ModsUC>();
            services.AddSingleton<StructureUC>();
            services.AddSingleton<ImagesUC>();

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            serviceProvider.GetService<MainWindow>().Show();
        }
        public ResourceDictionary ThemeDictionary
        {
            get { return Resources.MergedDictionaries[0]; }
        }
        public ResourceDictionary Localization
        {
            get { return Resources.MergedDictionaries[1]; }
        }

        public void ChangeTheme(Uri uri)
        {
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });            
        }
        public void ChangeLocalization(Uri uri)
        {
            Localization.MergedDictionaries.Clear();
            Localization.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }

    }
}
