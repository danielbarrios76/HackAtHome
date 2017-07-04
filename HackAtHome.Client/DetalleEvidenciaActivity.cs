
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Webkit;
using Android.Widget;
using HackAtHome.SAL;
using System.Net;

namespace HackAtHome.Client
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon")]
    public class DetalleEvidenciaActivity : Activity
    {
        FragmentEvidenceDetail evidenceDetail;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DetalleEvidencia);

            //instancias del contenido del Layout
            var txtNombre = FindViewById<TextView>(Resource.Id.textViewNombreDetalle);
            var txtTitle = FindViewById<TextView>(Resource.Id.textViewEvidenceTitle);
            var txtStatus = FindViewById<TextView>(Resource.Id.textViewStatus);
            var webDescription = FindViewById<WebView>(Resource.Id.webViewDescripcion);
            var imgDescription = FindViewById<ImageView>(Resource.Id.imageViewDescription);

            //Obtenemos los valores pasados de la activity
            string txtUsuario = Intent.GetStringExtra("Usuario");
            string txtTituloEvidencia = Intent.GetStringExtra("Titulo");
            string txtEstadoEvidencia = Intent.GetStringExtra("Estado");
            string txtToken = Intent.GetStringExtra("Token");
            int evidenceID = Intent.GetIntExtra("EvidenceID", 0);

            //recuperamos los datos persistidos
            evidenceDetail = (FragmentEvidenceDetail)this.FragmentManager.FindFragmentByTag("EvidenceDetail");

            if (evidenceDetail == null)
            {
                //Instancia de ServiceClient
                var Client = new ServiceClient();

                evidenceDetail = new FragmentEvidenceDetail();
                var FragmentTransition = this.FragmentManager.BeginTransaction();
                FragmentTransition.Add(evidenceDetail, "EvidenceDetail");
                FragmentTransition.Commit();

                //Obtenemos el detalle de la evidencia
                evidenceDetail.evidenceDetail = await Client.GetEvidenceByIDAsync(txtToken, evidenceID);
            }

            //Mostramos los datos de Usuario
            txtNombre.Text = txtUsuario;
            txtTitle.Text = txtTituloEvidencia;
            txtStatus.Text = txtEstadoEvidencia;

            string webViewContent = $"<div style='color:#BCBCBC'>{evidenceDetail.evidenceDetail.Description}</div>";

            webDescription.SetBackgroundColor(Color.Transparent);
            webDescription.LoadDataWithBaseURL(null, webViewContent, "text/html", "utf-8", null);
            imgDescription.SetImageBitmap(GetImageBitmapFromUrl(evidenceDetail.evidenceDetail.Url));

        }

        //Metodo privado para cargar una imagen desde una URL
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}