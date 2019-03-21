using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Com.OneSignal;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PSCMovel
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            OneSignal.Current.StartInit("de2f1261-8d66-40d4-a0dd-fe7ec1d0ba87")
                 .EndInit();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
