using App.Services;
using App.ViewModels;
using CommonServiceLocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<LoginViewModel>();
        }
    }
}