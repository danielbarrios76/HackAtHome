using Android.App;
using Android.OS;
using System.Collections.Generic;

namespace HackAtHome.Client
{
    class FragmentList : Fragment
    {
        public List<Entities.Evidence> listEvidence { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

    }
}