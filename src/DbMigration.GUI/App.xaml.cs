using DbMigration.GUI.MainWindow;
using DbMigrationTool.Application;
using DbMigrationTool.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace DbMigration.GUI
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration(SetupConfiguration)
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            _host.Services
                .GetRequiredService<MainWindowView>()
                .Show();
        }

        private void SetupConfiguration(IConfigurationBuilder configBuilder)
        {
            configBuilder
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false);
        }

        private void ConfigureServices(HostBuilderContext builderCtx, IServiceCollection services)
        {
            services
                .AddSingleton<MainWindowView>()
                .AddSingleton<MainWindowVm>()
                .AddApplicationLayer(builderCtx.Configuration)
                .AddInfrastructureLayer(builderCtx.Configuration);
        }
    }
}
