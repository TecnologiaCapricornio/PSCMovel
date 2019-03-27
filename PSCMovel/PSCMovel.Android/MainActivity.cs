using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.OneSignal;
using Acr.UserDialogs;
using Android.Content;

namespace PSCMovel.Droid
{
    [Activity(Label = "PSCMovel", Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            ISharedPreferences dados = Application.Context.GetSharedPreferences("PSC", Android.Content.FileCreationMode.Private);


            if (!dados.Contains("email")) // verificar se usuário já executou o app uma vez e se já inseriu e-mail
            {
                PromptResult pResult = null;
                // Se não existe e-mail cadastrado
                do
                {
                    UserDialogs.Init(this);

                    pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                    {
                        InputType = InputType.Name,
                        OkText = "Confirmar",
                        Title = "Insira seu e-mail para receber notificações:",
                    });

                    if(pResult.Ok && ( string.IsNullOrWhiteSpace(pResult.Text) || !pResult.Text.EndsWith("@capricornio.com.br")))
                    {
                        await UserDialogs.Instance.AlertAsync("O e-mail inserido é inválido");
                    }

                } while (pResult.Ok && !pResult.Text.EndsWith("@capricornio.com.br"));

                if (pResult.Ok)
                {
                    ISharedPreferencesEditor editor = dados.Edit();
                    editor.PutString("email", pResult.Text);
                    editor.Apply();

                    await UserDialogs.Instance.AlertAsync("Obrigado por se inscrever!");

                    OneSignal.Current.SetEmail(pResult.Text);
                    OneSignal.Current.RegisterForPushNotifications();
                    OneSignal.Current.StartInit("de2f1261-8d66-40d4-a0dd-fe7ec1d0ba87")
                        .EndInit();

                }
            }
            else
            {
                OneSignal.Current.SetEmail(dados.GetString("email", null));
                OneSignal.Current.RegisterForPushNotifications();
                OneSignal.Current.StartInit("de2f1261-8d66-40d4-a0dd-fe7ec1d0ba87")
                    .EndInit();
            }
        }
    }
}