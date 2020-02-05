using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace SportsEventCreator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    // [Android.Runtime.Register("android/content/Intent", ApiSince = 1, DoNotGenerateAcw = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
        }
    }
}