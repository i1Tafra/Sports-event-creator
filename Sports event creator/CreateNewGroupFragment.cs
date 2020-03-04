using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using SportsEventCreator.Database;
using System.Linq;
using Fragment = Android.Support.V4.App.Fragment;

namespace SportsEventCreator.Resources
{
    internal class CreateNewGroupFragment : Fragment
    {
        #region Properties and variables
        private EditText editName;
        //TOOD: Implement this on listView or recycleView
        private TextView twUsersList;

        private Button btnAddUser;
        private Button btnCreate;

        private Group group;
        private View view;
        #endregion

        private void InitGUIElements(View view)
        {
            twUsersList = view.FindViewById<TextView>(Resource.Id.txt_users);
            editName = view.FindViewById<EditText>(Resource.Id.edit_name);

            btnAddUser = view.FindViewById<Button>(Resource.Id.btn_add_user);
            btnCreate = view.FindViewById<Button>(Resource.Id.btn_create);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.fragment_create_new_group, container, false);
            group = new Group();
            InitGUIElements(view);
            InitBtnClickListeners();
            return view;
        }

        private void InitBtnClickListeners()
        {
            btnAddUser.Click += async (sender, e) =>
            {
                ShowAddUserDialog();
            };

            btnCreate.Click += async (sender, e) =>
            {
                //TODO: Validate that user does not have a group with a same name
                // prefer to validate as user type group name, disable Create button if validation fails
                group.Name = editName.Text;
                Instance.UserGroups.AddGroup(group);
                var updateTask = DatabaseManager.UpdateUserGroups(Instance.UserGroupsDocumentId, Instance.UserGroups);
                await updateTask.ConfigureAwait(true);

                if (updateTask.IsCompletedSuccessfully)
                {
                    Toast.MakeText(view.Context, Resource.String.info_firestore_success, ToastLength.Short)
                  .Show();
                }
                else
                {
                    Toast.MakeText(view.Context, Resource.String.err_failed_to_save_firestore, ToastLength.Short)
                  .Show();
                }
                //TODO: Switch to antoher view after group is added or clear to be ready for another group addition
            };
        }

        /// <summary>
        /// Show Simple input dialog with ability to confirm or cancel user addition to a group
        /// Search for a specified input (username or user email) on firestore.
        /// If found and if user is not already in a group users list add user to a list
        /// </summary>
        private void ShowAddUserDialog()
        {
            EditText input = new EditText(view.Context);

            var dialog = new AlertDialog.Builder(view.Context)
                .SetTitle(Resource.String.dialog_title_create_user)
                .SetPositiveButton(Resource.String.dialog_create_user, async (senderAlert, args) =>
                {
                    Plugin.CloudFirestore.IQuerySnapshot snapshot = await DatabaseManager.GetUser(input.Text)
                    .ConfigureAwait(true);

                    if (snapshot.IsEmpty)
                    {
                        Toast.MakeText(view.Context, Resource.String.info_user_not_found, ToastLength.Short).Show();
                        return;
                    }

                    User user = snapshot.ToObjects<User>().FirstOrDefault();
                    group.AddUser(user);

                    Toast.MakeText(view.Context, Resource.String.info_user_added, ToastLength.Short).Show();

                    twUsersList.Text = string.Empty;

                    group.Users.ForEach(u => { twUsersList.Text += $"{u.Username}\n"; });
                })
                .SetNegativeButton(Resource.String.dialog_cancel, (senderAlert, args) => { })
                .SetView(input)
                .Create();

            dialog.Show();
        }
    }
}