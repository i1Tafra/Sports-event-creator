using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SportsEventCreator
{
    public class AppSettings
    {
        private readonly ISharedPreferences sharedPrefs;
        private readonly ISharedPreferencesEditor prefsEditor;
        private readonly Context context;

        private const String USERNAME = "USERNAME";
        private const String PASSWORD = "PASSWORD";

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

            get => sharedPrefs.GetString(USERNAME, String.Empty);
        }

        public string Password
        {
            set
            {
                prefsEditor.PutString(PASSWORD, value);
                prefsEditor.Commit();
            }

            get => sharedPrefs.GetString(PASSWORD, String.Empty);
        }

    }
}