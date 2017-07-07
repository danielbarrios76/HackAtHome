using Android.App;
using Android.OS;
using Android.Widget;
using HackAtHome.Entities;
using HackAtHome.SAL;



namespace HackAtHome.Client
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var btnValidar = FindViewById<Button>(Resource.Id.btnValidate);
            var txtEmail = FindViewById<EditText>(Resource.Id.editTextEmail);
            var txtPassword = FindViewById<EditText>(Resource.Id.editTextPassword);


            btnValidar.Click += async (sender, e) =>
            {
                var MicrosoftEvidence = new LabItem
                {
                    Email = txtEmail.Text,
                    Lab = "Hack@Home",
                    DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId)
                };

                var MicrosoftClient = new MicrosoftServiceClient();
                await MicrosoftClient.SendEvidence(MicrosoftEvidence);

                //Llamamos a la Activity que mostrara el listado de evidencias
                var intentListaEvidencias = new Android.Content.Intent(this, typeof(ListaEvidenciasActivity));
                intentListaEvidencias.PutExtra("Usuario", txtEmail.Text);
                intentListaEvidencias.PutExtra("Clave", txtPassword.Text);
                StartActivity(intentListaEvidencias);
                
            };
        }
    }
}

