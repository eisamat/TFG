using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using App.Database;
using App.Services;
using App.Services.Api;
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
        
        protected async override void OnStart()
        {
            var database = await AppDatabase.Instance;
            var loggedUser = await database.GetUserAsync();

            if (loggedUser == null)
            {
                // Show Login page
                Console.WriteLine("User not logged in");
            }
            else
            {
                // Show dashboard page
                Console.WriteLine("User logged in");
                await Shell.Current.GoToAsync("///MainPage");
            }
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
            
        }
        
        private static void ConfigureRestApi(IUnityContainer container)
        {
            var client = new HttpClient(container.Resolve<AuthorizationHttpHandler>())
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
