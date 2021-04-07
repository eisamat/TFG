using System;
using App.Database;
using App.Models;
using App.Services;
using App.Services.Api;
using Refit;
using Shared.Api.Requests;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _token;

        public Command LoginCommand { get; }

        private readonly IRestApi _restApi;
        private readonly ILoadingPageService _loadingPageService;
        
        public LoginViewModel(IRestApi restApi, ILoadingPageService loadingPageService)
        {
            _restApi = restApi;

            LoginCommand = new Command(OnLoginClicked, LoginAllowed);

            _loadingPageService = loadingPageService;
        }
        
        public string Token
        {
            get => _token;
            set
            {
                SetProperty(ref _token, value);
                LoginCommand.ChangeCanExecute();
            }
        }

        private bool LoginAllowed(object obj) => !string.IsNullOrEmpty(_token);
        
        private async void OnLoginClicked(object obj)
        {
            _loadingPageService.ShowLoadingPage();

            try
            {
                var request = new PatientLoginRequest(_token);
                var response = await _restApi.Authenticate(request);
                var database = await AppDatabase.Instance;
                await database.SaveUserAsync(new User
                {
                    Id = response.Id,
                    Name = response.Name,
                    Nhc = response.Nhc,
                    Therapist = response.Therapist,
                    Token = response.Token,
                    Zip = response.Zip
                });
            }
            catch (ApiException e)
            {
                var statusCode = e.StatusCode;
                Console.WriteLine("Error: " + (int)statusCode);
            }
            finally
            {
                _loadingPageService.HideLoadingPage();    
            }
        }
    }
}
