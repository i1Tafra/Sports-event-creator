using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;

namespace SportsEventCreator.Resources
{
    class EventsFragment : Fragment
    {
       public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
       {

            return inflater.Inflate(Resource.Layout.fragment_events, container, false);

       }
    }
}