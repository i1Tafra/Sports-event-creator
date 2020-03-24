using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Firebase;
using SportsEventCreator.Database;
using SportsEventCreator.Firebase;
using System;
using Xamarin.Essentials;

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
            btnSignIn.Enabled = true;
            btnSignIn.Enabled = false;
            btnSignIn.Enabled = true;
        }

        private void InitFirebase()
        {
            FirebaseHandler.Init(this);
        }

        private async void TEST()
        {
            // for testing
            SportEvent sevent = new SportEvent()
            {
                Date = DateTime.Now,
                Location = "Iza moje kuce 46",
                EventType = SportType.Basketball,
                Creator = Instance.User
            };
            sevent.AddUser(new User("Josip", "joko@toki.com"));
            sevent.AddUser(new User("Toki", "joko@toki.com"));
            sevent.AddUser(new User("Karlo", "joko@toki.com"));
            sevent.AddUser(new User("Zvonki", "joko@toki.com"));
            sevent.AddUser(new User("Karlo", "joko@toki.com"));
            sevent.AddUser(Instance.User);
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
        }

        private void InitBtnClickListeners()
        {
            btnSignIn.Click += (sender, e) =>
            {
                btnSignIn.Enabled = false;
                if (ValidateInput())
                    LogInUser();
                btnSignIn.Enabled = true;
            };

            btnDelete.Click += (sender, e) =>
            {
                SaveUsernamePassword(string.Empty, string.Empty);
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
            if (string.IsNullOrEmpty(editUsername.Text) 
                || string.IsNullOrEmpty(editPassword.Text))
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
                Instance.LoadUserData(editUsername.Text);

                TEST();
                StartActivity(typeof(MainActivity));
                Finish();
            }
            catch (FirebaseException excepion)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                   Toast.MakeText(ApplicationContext, excepion.Message, ToastLength.Short).Show();
                });

            }
        }

        /// <summary>
        /// Save username and password in application settings so next time user can easly login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void SaveUsernamePassword(string username, string password)
        {
            _ = new AppSettings(this)
            {
                Username = username,
                Password = password
            };
        }

        /// <summary>
        /// Load previously saved username and password from user, or String.Empty on GUI elements
        /// </summary>
        private void LoadUsernamePassword()
        {
            AppSettings appSetting = new AppSettings(this);
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