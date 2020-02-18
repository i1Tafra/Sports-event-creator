using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Firebase.Auth;
using Firebase;
using Android.Support.Design.Widget;
using Android.Support.Constraints;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using SportsEventCreator.Firebase;
using Xamarin.Essentials;
using Plugin.CloudFirestore;
using SportsEventCreator.Database;
using System.Collections.Generic;

namespace SportsEventCreator
{
    [Activity(Label = "LogInActivity", MainLauncher = true)]
    public class LogInActivity : Activity
    {
        #region Properties and variables
        private EditText editUsername;
        private EditText editPassword;

        private Button btnSignIn;
        private Button btnDelete;
        private Button btnForgetPass;
        private Button btnRegister;

        #endregion

        private void InitGUIElements()
        {
            editUsername = FindViewById<EditText>(Resource.Id.edit_username);
            editPassword = FindViewById<EditText>(Resource.Id.edit_password);

            btnSignIn = FindViewById<Button>(Resource.Id.btn_sign_in);
            btnDelete = FindViewById<Button>(Resource.Id.btn_delete);
            btnForgetPass = FindViewById<Button>(Resource.Id.btn_password_forgot);
            btnRegister = FindViewById<Button>(Resource.Id.btn_register);

        }

        private void InitFirebase()
        {
            FirebaseHandler.Init(this);
        }

        private async void TEST()
        {
            // for testing
            var sevent = new SportEvent()
            {
                Date = DateTime.Now,
                Location = "Iza moje kuce 34",
                EventType = SportType.Basketball,
                Creator = new User("Bonm", "jakov@hotmail.com")
            };
            sevent.AddUser(new User("Josip", "joko@toki.com"));
            sevent.AddUser(new User("Toki", "joko@toki.com"));
            sevent.AddUser(new User("Karlo", "joko@toki.com"));
            sevent.AddUser(new User("Zvonki", "joko@toki.com"));
            sevent.AddUser(new User("Karlo", "joko@toki.com"));
            await DatabaseManager.AddSportEvent(sevent).ConfigureAwait(false);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            InitFirebase();
            InitGUIElements();
            InitBtnClickListeners();
            LoadUsernamePassword();
           // TEST();
        }

        private void InitBtnClickListeners()
        {
            btnSignIn.Click += (sender, e) =>
            {
                if (ValidateInput())
                    LogInUser();
            };

            btnDelete.Click += (sender, e) =>
            {
                SaveUsernamePassword(String.Empty, String.Empty);
                Toast.MakeText(ApplicationContext, Resource.String.info_deleted, ToastLength.Short)
                .Show();
            };

            btnForgetPass.Click += (sender, e) =>
            {
                Toast.MakeText(ApplicationContext, Resource.String.warn_implementation_need, ToastLength.Short)
                .Show();
            };

            btnRegister.Click += (sender, e) =>
            {
                StartActivity(typeof(RegisterActivity));
            };
        }

        private bool ValidateInput()
        {
            if (String.IsNullOrEmpty(editUsername.Text) ||
                String.IsNullOrEmpty(editPassword.Text))
            {
                Toast.MakeText(ApplicationContext, Resource.String.err_empty, ToastLength.Short)
                .Show();
                return false;
            }
            return true;
        }

        private async void LogInUser()
        {
            try
            {
                await FirebaseHandler.Instance.Auth
               .SignInWithEmailAndPasswordAsync(editUsername.Text, editPassword.Text)
               .ConfigureAwait(false);

                SaveUsernamePassword(editUsername.Text, editPassword.Text);
                StartActivity(typeof(MainActivity));
                Finish();
            }
            catch (FirebaseException excepion)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Toast.MakeText(ApplicationContext, excepion.Message, ToastLength.Short)
                  .Show();
                });

            }
        }

        private void SaveUsernamePassword(string username, string password)
        {
            _ = new AppSettings(this)
            {
                Username = username,
                Password = password
            };
        }

        private void LoadUsernamePassword()
        {
            var appSetting = new AppSettings(this);
            editUsername.Text = appSetting.Username;
            editPassword.Text = appSetting.Password;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}