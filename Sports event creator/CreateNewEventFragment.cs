using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace SportsEventCreator.Resources
{
    internal class CreateNewEventFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            return inflater.Inflate(Resource.Layout.fragment_create_new_event, container, false);

        }
    }
}