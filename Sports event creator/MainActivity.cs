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

namespace SportsEventCreator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
   // [Android.Runtime.Register("android/content/Intent", ApiSince = 1, DoNotGenerateAcw = true)]
    public class MainActivity : AppCompatActivity, IOnCompleteListener
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            InitFirebase();
            InitGUIElements();
            InitBtnClickListeners();
        }

        private void InitBtnClickListeners()
        {
            btnSignIn.Click += (sender, e) => {
                //TODO: handle empty strings
                var task = FirebaseHandler.Instance.Auth.SignInWithEmailAndPassword(editUsername.Text, editPassword.Text)
                .AddOnCompleteListener(this);
            };

            btnDelete.Click += (sender, e) => {
                Toast.MakeText(ApplicationContext, Resource.String.warn_implementation_need, ToastLength.Short)
                .Show();
            };

            btnForgetPass.Click += (sender, e) => {
                Toast.MakeText(ApplicationContext, Resource.String.warn_implementation_need, ToastLength.Short)
                .Show();
            };

            btnRegister.Click += (sender, e) => {
                StartActivity(typeof(RegisterActivity));
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (!task.IsSuccessful)
                Toast.MakeText(ApplicationContext, Resource.String.err_login_fail, ToastLength.Short)
                    .Show();
            else
                Toast.MakeText(ApplicationContext, Resource.String.warn_implementation_need, ToastLength.Short)
                    .Show();
        }
    }
}