using DbMigration.GUI.Context;
using DbMigration.GUI.MainWindow;
using DbMigrationTool.Application;
using DbMigrationTool.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
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
                .ConfigureLogging(ConfigureLogging)
                .Build();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            _host
                .Services
                .GetRequiredService<ILogger<App>>()
                .LogInformation("Starting Application################################################");

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
                .AddSingleton<ContextViewVm>()
                .AddSingleton<ApplyMigrationCmd>()
                .AddApplicationLayer(builderCtx.Configuration)
                .AddInfrastructureLayer(builderCtx.Configuration);
        }

        private void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .SetMinimumLevel(LogLevel.Information)
                .AddNLog()
                .AddFilter("Microsoft.EntityFrameworkCore.*", LogLevel.Warning);
        }
    }
}
