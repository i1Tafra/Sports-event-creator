using System;
using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Views;
using SportsEventCreator.Resources;

namespace SportsEventCreator
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    // [Android.Runtime.Register("android/content/Intent", ApiSince = 1, DoNotGenerateAcw = true)]
    public class MainActivity : AppCompatActivity , NavigationView.IOnNavigationItemSelectedListener
    {
        private DrawerLayout drawer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);   //set toolbar as action bar, this will put title of our activity in toolbar or place options menu if we create one

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //to listen to click events on our navigation view we need to reference navigation view
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open,
                                                                     Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState(); //rotating the hamburger icon

            if (savedInstanceState == null) { 
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, new EventsFragment()).Commit();
            navigationView.SetCheckedItem(Resource.Id.nav_events);
            }
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Resource.Id.nav_events:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, new EventsFragment()).Commit();
                    break;
                case Resource.Id.nav_my_groups:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, new MyGroupsFragment()).Commit();
                    break;
                case Resource.Id.nav_create_new_event:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, new CreateNewEventFragment()).Commit();
                    break;
                case Resource.Id.nav_create_new_group:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, new CreateNewGroupFragment()).Commit();
                    break;
                case Resource.Id.nav_add_to_group:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, new AddToGroupFragment()).Commit();
                    break;
                
            }
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        // when we press back button when navigation drawer is opened we don't want to leave the activity immediately, instead we want to close drawer
        public override void OnBackPressed()
        {
            if (drawer.IsDrawerOpen(GravityCompat.Start))
                drawer.CloseDrawer(GravityCompat.Start);
            else
                base.OnBackPressed();
        }

        
    }
}