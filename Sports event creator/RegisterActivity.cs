using Android.App;
//using Android.Gms.Tasks;
using Android.OS;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using SportsEventCreator.Database;
using SportsEventCreator.Firebase;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SportsEventCreator
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        #region Properties and variables
        private EditText editUsername;
        private EditText editPassword;
        private EditText editPasswordRepeat;
        private EditText editMail;
        private EditText editMailRepeat;

        private Button btnSumbit;

        private const uint MIN_PASSWORD_LENGTH = 8;
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
            btnSumbit.Click += (sender, e) =>
            {
                btnSumbit.Enabled = false;
                if (ValidateInput() && ValidatePasswordComplexity())
                    RegisterUser();
                btnSumbit.Enabled = true;

                ///TODO: Research how to implement email verification
                /*using var user = authResult.User;
                using var actionCode = ActionCodeSettings.NewBuilder()
                .SetAndroidPackageName(PackageName, true, "0")
                .SetUrl("https://sport-event-creator.firebaseapp.com/__/auth/action")
                .Build();
                await user.SendEmailVerificationAsync(actionCode).ConfigureAwait(true);*/
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "I am using 'using' statement, variable should be disposed automatically")]
        private async void RegisterUser()
        {
            try
            {
                //editMail.Text = "test_16@test.com";
                //editUsername.Text = "TESTuSER";
                //editPassword.Text = "Tafra123";
                using IAuthResult authResult = await FirebaseHandler.Instance.Auth
                .CreateUserWithEmailAndPasswordAsync(editMail.Text, editPassword.Text)
                .ConfigureAwait(false);

                using UserProfileChangeRequest profile = new UserProfileChangeRequest.Builder()
               .SetDisplayName(editUsername.Text)
               .Build();

                await authResult.User
                .UpdateProfileAsync(profile)
                .ConfigureAwait(false);

                UserProfile up = new UserProfile(editUsername.Text, editMail.Text);
                UserGroups ug = new UserGroups()
                {
                    Creator = new User(up)
                };

                System.Collections.Generic.List<Task> tasks = Instance.InitFirestoreUser(editUsername.Text, editMail.Text);
                Task.WaitAll(tasks.ToArray());
                if (tasks.All(t => t.IsCompletedSuccessfully))
                {
                    Instance.LoadUserData(editUsername.Text);
                    StartActivity(typeof(MainActivity));
                    Finish();
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Toast.MakeText(ApplicationContext, Resource.String.err_failed_to_save_firestore, ToastLength.Short)
                      .Show();
                    });
                }
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

        private bool ValidatePasswordComplexity()
        {
            string pass = editPassword.Text;

            bool result = (pass.Any(char.IsUpper) &&
                pass.Any(char.IsLower) &&
                pass.Any(char.IsDigit) &&
                pass.Length >= MIN_PASSWORD_LENGTH);

            if (!result)
            {
                Toast.MakeText(ApplicationContext, Resource.String.err_weak_pass, ToastLength.Short)
                .Show();
            }

            return result;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(editUsername.Text) ||
                string.IsNullOrEmpty(editPassword.Text) ||
                string.IsNullOrEmpty(editMail.Text))
            {
                Toast.MakeText(ApplicationContext, Resource.String.err_empty, ToastLength.Short)
                .Show();
                return false;
            }

            if (!editPassword.Text.Equals(editPasswordRepeat.Text, StringComparison.Ordinal) ||
              !editMail.Text.Equals(editMailRepeat.Text, StringComparison.Ordinal))
            {
                Toast.MakeText(ApplicationContext, Resource.String.err_not_match, ToastLength.Short)
                .Show();
                return false;
            }
            return true;
        }
    }
}