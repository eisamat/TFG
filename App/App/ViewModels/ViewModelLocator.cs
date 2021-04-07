using CommonServiceLocator;

namespace App.ViewModels
{
    public class ViewModelLocator
    {
        public LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();
    }
}