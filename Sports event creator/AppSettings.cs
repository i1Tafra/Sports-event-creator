using Android.Content;
using Android.Preferences;

namespace SportsEventCreator
{
    public class AppSettings
    {
        private readonly ISharedPreferences sharedPrefs;
        private readonly ISharedPreferencesEditor prefsEditor;
        private readonly Context context;

        private const string USERNAME = "USERNAME";
        private const string PASSWORD = "PASSWORD";

        public AppSettings(Context context)
        {
            this.context = context;
            sharedPrefs = PreferenceManager.GetDefaultSharedPreferences(this.context);
            prefsEditor = sharedPrefs.Edit();
        }

        public string Username
        {
            set
            {
                prefsEditor.PutString(USERNAME, value);
                prefsEditor.Commit();
            }

            get => sharedPrefs.GetString(USERNAME, string.Empty);
        }

        public string Password
        {
            set
            {
                prefsEditor.PutString(PASSWORD, value);
                prefsEditor.Commit();
            }

            get => sharedPrefs.GetString(PASSWORD, string.Empty);
        }

    }
}