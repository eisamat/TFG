using App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _mainViewModel;
        
        public MainPage()
        {
            InitializeComponent();
            _mainViewModel = (MainViewModel) BindingContext;
        }

        protected override void OnAppearing()
        {
            _mainViewModel.GetPatientDetails.Execute(null);
            base.OnAppearing();
        }
    }
}