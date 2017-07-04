using Android.App;
using Android.OS;

namespace HackAtHome.Client
{
    class FragmentResult : Fragment
    {
        public Entities.ResultInfo result { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
    }
}