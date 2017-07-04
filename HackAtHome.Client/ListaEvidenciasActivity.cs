using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Linq;

namespace HackAtHome.Client
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon")]
    public class ListaEvidenciasActivity : Activity
    {
        FragmentList listEvidences;
        FragmentResult resultInfo;

        protected async override void OnCreate(Bundle saveStateInstance)
        {
            base.OnCreate(saveStateInstance);
            SetContentView(Resource.Layout.ListaEvidencias);

            
            //Obtenemos los valores de Usuario y clave de la Activity anterior
            string txtUsuario = Intent.GetStringExtra("Usuario");
            string txtClave = Intent.GetStringExtra("Clave");

            
            //Instanciamos las variables
            var listViewEvidences = FindViewById<ListView>(Resource.Id.listViewEvidence);
            var txtNombreUsuario = FindViewById<TextView>(Resource.Id.textViewNombre);

            //Instancia de los servicios de validacion
            var Client = new SAL.ServiceClient();

            //recuperamos los datos persistidos de validacion
            //si no existen los obtenemos

            resultInfo = (FragmentResult)this.FragmentManager.FindFragmentByTag("ResultInfo");
            if (resultInfo == null)
            {
                resultInfo = new FragmentResult();
                var FragmentTransition = this.FragmentManager.BeginTransaction();
                FragmentTransition.Add(resultInfo, "ResultInfo");
                FragmentTransition.Commit();
                resultInfo.result = await Client.AutenticateAsync(txtUsuario, txtClave);
            }

            txtNombreUsuario.Text = resultInfo.result.FullName;

            //Recuperamos datos persistidos de la lista
            //si no existen obtenemos el contnido de la lista
            listEvidences = (FragmentList)this.FragmentManager.FindFragmentByTag("ListEvidences");
            if (listEvidences == null)
            {
                listEvidences = new FragmentList();
                var FragmentTransition = this.FragmentManager.BeginTransaction();
                FragmentTransition.Add(listEvidences, "ListEvidences");
                FragmentTransition.Commit();

                listEvidences.listEvidence = await Client.GetEvidencesAsync(resultInfo.result.Token);
            }

            listViewEvidences.Adapter = new CustomAdapters.EvidencesAdapter(this, listEvidences.listEvidence,
                                                                        Resource.Layout.ListItem,
                                                                        Resource.Id.textViewLab,
                                                                        Resource.Id.textViewResult);




            listViewEvidences.ItemClick += (sender, e) =>
            {
                //Llamamos a la Activity que mostrara el detalle de evidencias
                var intentListaEvidencias = new Intent(this, typeof(DetalleEvidenciaActivity));
                int itemPosition = e.Position;
                Entities.Evidence evidence = (Entities.Evidence)listEvidences.listEvidence.ElementAt(itemPosition);

                //Pasamos los valores para la siguiente activity
                intentListaEvidencias.PutExtra("Usuario", resultInfo.result.FullName);
                intentListaEvidencias.PutExtra("Titulo", evidence.Title);
                intentListaEvidencias.PutExtra("Estado", evidence.Status);
                intentListaEvidencias.PutExtra("Token", resultInfo.result.Token);
                intentListaEvidencias.PutExtra("EvidenceID", evidence.EvidenceID);
                StartActivity(intentListaEvidencias);

            };


        }
    }

}