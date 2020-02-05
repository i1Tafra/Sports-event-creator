using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SportsEventCreator.Firebase;

namespace SportsEventCreator
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity, IOnCompleteListener
    {
        #region Properties and variables
        private EditText editUsername;
        private EditText editPassword;
        private EditText editPasswordRepeat;
        private EditText editMail;
        private EditText editMailRepeat;

        private Button btnSumbit;


        //private static FirebaseApp app;
        //private FirebaseAuth auth;
        #endregion

        private void InitGUIElements()
        {
            editUsername = FindViewById<EditText>(Resource.Id.edit_username);
            editPassword = FindViewById<EditText>(Resource.Id.edit_password);
            editPasswordRepeat = FindViewById<EditText>(Resource.Id.edit_password_repeat);
            editMail = FindViewById<EditText>(Resource.Id.edit_email);
            editMailRepeat = FindViewById<EditText>(Resource.Id.edit_email_repeat);

            btnSumbit = FindViewById<Button>(Resource.Id.btn_submit);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_registration);

            InitGUIElements();
            InitBtnClickListeners();
        }

        private void InitBtnClickListeners()
        {
            btnSumbit.Click += (sender, e) => {
                if (ValidateInput())
                {
                    var task = FirebaseHandler.Instance.Auth
                    .CreateUserWithEmailAndPassword(editMail.Text, editPassword.Text)
                    .AddOnCompleteListener(this);
                }
            };
        }

        private bool ValidateInput()
        {
            if(String.IsNullOrEmpty(editUsername.Text) ||
                String.IsNullOrEmpty(editPassword.Text) ||
                String.IsNullOrEmpty(editMail.Text))
            {
                Toast.MakeText(ApplicationContext, Resource.String.err_empty, ToastLength.Short)
                .Show();
                return false;
            }

            if(!editPassword.Text.Equals(editPasswordRepeat.Text) ||
              !editMail.Text.Equals(editMailRepeat.Text))
            {
                Toast.MakeText(ApplicationContext, Resource.String.err_not_match, ToastLength.Short)
                .Show();
                return false;
            }
            return true;
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