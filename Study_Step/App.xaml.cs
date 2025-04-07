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
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCekx3RHxbf1x1ZFZMYFxbQXFPIiBoS35Rc0VnWXtfdXRVQ2dYUUR+VEBU");
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
            
            services.AddSingleton<ITokenStorage, SecureTokenStorage>();
            services.AddSingleton<AuthService>();
            services.AddSingleton<SignalRService>();

            services.AddSingleton<AuthViewModel>(); 
            services.AddSingleton<ViewModel>();

            services.AddTransient<AuthWindow>(); 
            services.AddTransient<MainWindow>();

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
                Debug.WriteLine("test1");
                var loginWindow = ServiceProvider.GetRequiredService<AuthWindow>();
                loginWindow.Show();
                Debug.WriteLine("test2");
            }
        }
    }

}
