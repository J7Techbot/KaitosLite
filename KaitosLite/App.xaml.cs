
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
            services.AddSingleton<WindowManager>();
            /// Managers
            services.AddSingleton<ScanManager>();
            services.AddSingleton<PageManager>();
            services.AddSingleton<ModsManager>();
            services.AddSingleton<ProjectManager>();
            services.AddSingleton<ProjectUC>();

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {

            IocResolver.Resolve = serviceProvider.GetService;

            var navigationService = serviceProvider.GetService<WindowManager>();
           
            navigationService.Show<MainWindow>();
        }
    }
}
