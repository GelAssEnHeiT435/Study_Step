using Microsoft.Extensions.DependencyInjection;
using Study_Step.Data;
using Study_Step.Interfaces;
using Study_Step.Pages;
using Study_Step.Services;
using Study_Step.ViewModels;
using Study_Step.Services;
using System.Configuration;
using System.Data;
using System.Windows;
using AutoMapper;

namespace Study_Step
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } // DI-контейнер

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Настройка DI-контейнера
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Создание DI-контейнера
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Запуск главного окна
            var loginWindow = ServiceProvider.GetRequiredService<AuthWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SynchronizationContext>(SynchronizationContext.Current);
            // Регистрация сервисов и ViewModel
            services.AddSingleton<SignalRService>(); 

            services.AddSingleton<AuthViewModel>(); 
            services.AddSingleton<ViewModel>();

            services.AddTransient<AuthWindow>(); 
            services.AddTransient<MainWindow>();

            services.AddTransient<RegisterPage>(); 
            services.AddTransient<SignInPage>();

            // Add converter models to data transfer object
            services.AddAutoMapper(typeof(ClientMapperProfile).Assembly);
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<DtoConverterService>();
        }
    }

}
