using System;
using System.Net.Http;
using App.Services;
using App.Services.Api;
using App.Views;
using CommonServiceLocator;
using Refit;
using Unity;
using Unity.ServiceLocation;
using Xamarin.Forms;

namespace App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            
            var container = new UnityContainer();
            
            ConfigureRestApi(container);
            
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
            
            MainPage = new AppShell();
            
            ConfigureLoadingPageService(container);
        }
        
        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
            
        }
        
        private static void ConfigureRestApi(IUnityContainer container)
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true
            };
            var client = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri(Urls.BaseUrl)
            };

            var restApi = RestService.For<IRestApi>(client);

            container.RegisterInstance(restApi);
        }
        
        private static void ConfigureLoadingPageService(IUnityContainer container)
        {
            var loadingPageService = DependencyService.Get<ILoadingPageService>();
            container.RegisterInstance(loadingPageService);
        }
    }
}
