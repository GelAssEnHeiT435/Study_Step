using Microsoft.Extensions.DependencyInjection;
using Study_Step.Data;
using Study_Step.Interfaces;
using Study_Step.Pages;
using Study_Step.Services;
using Study_Step.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Diagnostics;
using System.Windows.Interop;
using System.Windows.Media;
using Study_Step.UI.Windows;

namespace Study_Step
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; } // DI-контейнер
        public IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Properties\\appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF1cWWhPYVJzWmFZfVtgd19DY1ZRQGYuP1ZhSXxWdkBiX39ddXBVTmhbWU19XUs=");
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            InitApplicationAsync().ConfigureAwait(false);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SynchronizationContext>(SynchronizationContext.Current);
            // Регистрация сервисов и ViewModel

            services.AddSingleton<UserSessionService>();
            services.AddSingleton<ITokenStorage, SecureTokenStorage>();
            services.AddSingleton<AuthService>();
            services.AddSingleton<SignalRService>();

            services.AddSingleton<AuthViewModel>(); 
            services.AddSingleton<ViewModel>();

            services.AddSingleton<AuthWindow>(); 
            services.AddSingleton<MainWindow>();
            services.AddTransient<DeletionWindow>();

            services.AddTransient<RegisterPage>(); 
            services.AddTransient<SignInPage>();

            // Add converter models to data transfer object
            services.AddAutoMapper(typeof(ClientMapperProfile).Assembly);
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<DtoConverterService>();
        }

        private async Task InitApplicationAsync()
        {
            var authServer = ServiceProvider.GetRequiredService<AuthService>();
            bool isLoggedIn = await authServer.TryAutoLoginAsync();

            // On UI-Stream
            if (isLoggedIn)
            {
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            else
            {
                var loginWindow = ServiceProvider.GetRequiredService<AuthWindow>();
                loginWindow.Show();
            }
        }
    }

}
