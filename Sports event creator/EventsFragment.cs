﻿using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace SportsEventCreator.Resources
{
    internal class EventsFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            return inflater.Inflate(Resource.Layout.fragment_events, container, false);

        }
    }
}