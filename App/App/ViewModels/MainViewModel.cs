using System;
using System.Threading.Tasks;
using App.Services;
using App.Services.Api;
using Refit;
using Shared.Api.Responses;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class MainViewModel: BaseViewModel
    {
        private PatientDetailsResponse _patientDetails;
        
        private readonly IRestApi _restApi;
        private readonly ILoadingPageService _loadingPageService;
        
        public Command GetPatientDetails { get; }

        public MainViewModel(IRestApi restApi, ILoadingPageService loadingPageService)
        {
            _restApi = restApi;
            _loadingPageService = loadingPageService;
            GetPatientDetails = new Command(OnPatientDetailsCalled);

            _restApi.ToString();
            //Task.Run(async () => { await GetDetailsRequest(); }).Wait();
        }
        
        public PatientDetailsResponse PatientDetails
        {
            get => _patientDetails;
            set
            {
                SetProperty(ref _patientDetails, value);
            }
        }
        
        private async void OnPatientDetailsCalled(object obj)
        {
            _loadingPageService.ShowLoadingPage();

            try
            {
                var response = await _restApi.PatientDetails();
                PatientDetails = response;
                Console.WriteLine("Response: " + response);
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