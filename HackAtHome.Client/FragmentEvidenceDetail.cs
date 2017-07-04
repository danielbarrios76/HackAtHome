
using Android.App;
using Android.OS;

namespace HackAtHome.Client
{
    class FragmentEvidenceDetail : Fragment
    {
        public Entities.EvidenceDetail evidenceDetail { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
    }
}