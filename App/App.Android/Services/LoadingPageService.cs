using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using App.Droid.Services;
using App.Services;
using App.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(LoadingPageService))]
namespace App.Droid.Services
{
    public class LoadingPageService : ILoadingPageService
    {
        private Android.Views.View _nativeView;

        private Dialog _dialog;

        private bool _isInitialized;

        public LoadingPageService()
        {
            
        }

        public void InitLoadingPage(ContentPage loadingIndicatorPage)
        {
            // check if the page parameter is available
            if (loadingIndicatorPage == null) return;
            // build the loading page with native base
            loadingIndicatorPage.Parent = Xamarin.Forms.Application.Current.MainPage;

            loadingIndicatorPage.Layout(new Rectangle(0, 0,
                Xamarin.Forms.Application.Current.MainPage.Width,
                Xamarin.Forms.Application.Current.MainPage.Height));

            var renderer = Platform.CreateRendererWithContext(loadingIndicatorPage, Android.App.Application.Context);

            _nativeView = renderer.View;

            _dialog = new Dialog(Xamarin.Essentials.Platform.CurrentActivity);
            _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            _dialog.SetCancelable(false);
            _dialog.SetContentView(_nativeView);
            var window = _dialog.Window;
            if (window != null)
            {
                window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                window.ClearFlags(WindowManagerFlags.DimBehind);
                window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
            }

            _isInitialized = true;
        }

        public void ShowLoadingPage()
        {
            // check if the user has set the page or not
            if (!_isInitialized)
                InitLoadingPage(new LoadingIndicatorPage());

            // showing the native loading page
            _dialog.Show();
        }

        public void HideLoadingPage()
        {
            // Hide the page
            _dialog.Hide();
        }
    }

}