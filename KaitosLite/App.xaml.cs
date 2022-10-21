﻿
using DomainLayer.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewLayer.Managers;
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
            services.AddSingleton<WindowManager>();
            services.AddSingleton<ComponentsManager>();
            services.AddSingleton<ScanManager>();
            services.AddSingleton<PageManager>();
            services.AddSingleton<ModsManager>();
            services.AddSingleton<ProjectManager>();
            services.AddSingleton<WindowsConfigManager>();

            //ViewModels
            services.AddSingleton<DockerViewModel>();
            services.AddSingleton<MainViewModel>();

            //Windows
            services.AddSingleton<MainWindow>();

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            serviceProvider.GetService<MainWindow>().Show();

            //var navigationService = serviceProvider.GetService<WindowManager>();
           
            //navigationService.Show<MainWindow>();
        }
    }
}
