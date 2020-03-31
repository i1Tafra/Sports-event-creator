using Android.App;
using Android.Content;
using Android.OS;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using SportsEventCreator.Database;
using System;
using System.Linq;
using Android.Util;
using Fragment = Android.Support.V4.App.Fragment;
using System.Globalization;
using Android.Gms.Tasks;

namespace SportsEventCreator.Resources
{
    internal class CreateNewEventFragment : Fragment
    {
        #region Properties and variables
        private SportEvent sportEvent;
        private View view;
        private Spinner spSports;
        private EditText editLocation;
        private Button btnDate;
        private Button btnAddUser;
        private Button btnCreate;
        #endregion
        private void InitGUIElements(View view)
        {
            editLocation = view.FindViewById<EditText>(Resource.Id.edit_location);
            btnDate = view.FindViewById<Button>(Resource.Id.btn_date);
            btnAddUser = view.FindViewById<Button>(Resource.Id.btn_add_users);
            btnCreate = view.FindViewById<Button>(Resource.Id.btn_create);
            spSports = view.FindViewById<Spinner>(Resource.Id.spinner_sports);

            var sportTypes = SportType.GetValues(typeof(SportType));
            var sportTypesArray = sportTypes.Cast<SportType>().Select(e => e.ToString()).ToArray();
            var adapter = new ArrayAdapter<string>(view.Context, Android.Resource.Layout.SimpleSpinnerItem, sportTypesArray);
            spSports.Adapter = adapter;

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.fragment_create_new_event, container, false);
            sportEvent = new SportEvent();
            InitGUIElements(view);
            InitBtnClickListeners();
            return view;
        }

        private void InitBtnClickListeners()
        {
            btnDate.Click += (sender, e) =>
            {
                ShowDateTimeDialog();
            };

            btnAddUser.Click += (sender, e) =>
            {
                ChooseGroupDialog();
            };

            btnCreate.Click += async (sender, e) =>
            {
                sportEvent.Creator = new User(Instance.User);
                sportEvent.AddUser(Instance.User);
                sportEvent.Location = editLocation.Text;
                var addEventTask = DatabaseManager.AddSportEvent(sportEvent);
                await addEventTask.ConfigureAwait(true);

                if (addEventTask.IsCompletedSuccessfully)
                {
                    var snapshot = await DatabaseManager.GetSportEvents(sportEvent)
                                    .ConfigureAwait(true);
                    Instance.Events.Add(snapshot.ToObjects<SportEvent>().FirstOrDefault());

                    Toast.MakeText(view.Context, Resource.String.info_firestore_success, ToastLength.Short)
                  .Show();
                }
                else
                {
                    Toast.MakeText(view.Context, Resource.String.err_failed_to_save_firestore, ToastLength.Short)
                  .Show();
                }
            };

            spSports.ItemSelected += (sender, e) =>
            {
                string item = (string)spSports.GetItemAtPosition(e.Position);
                sportEvent.EventType = (SportType) Enum.Parse(typeof(SportType), item);
            };
        }

        private void ChooseGroupDialog()
        {
            var dialogView = LayoutInflater.Inflate(Resource.Layout.items_list, null);
            var lw_items = dialogView.FindViewById<ListView>(Resource.Id.lw_items);
            var items = Instance.UserGroups.Groups;
            var adapter = new ArrayAdapter<Group>(view.Context, Android.Resource.Layout.SimpleListItem1, items);
            lw_items.Clickable = true;
            lw_items.Adapter = adapter;

            lw_items.ItemClick += (sender, e) =>
            {
                items[e.Position].Users.ForEach(u => sportEvent.AddUser(u));
            };

            using var dialog = new Android.App.AlertDialog.Builder(view.Context);
            dialog.SetTitle("Add users from group");
            dialog.SetView(dialogView);
            dialog.SetPositiveButton("OK", (sender, e) => {});
            var chooseGroup = dialog.Create();
            chooseGroup.Show();
        }

        public void ShowDateTimeDialog()
        {

            DateTime currently = DateTime.Now;
            DatePickerDialog dateDialog = new DatePickerDialog(view.Context,
                                                               OnDateSet,
                                                               currently.Year,
                                                               currently.Month - 1,
                                                               currently.Day);

            dateDialog.DatePicker.MinDate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            dateDialog.DatePicker.MaxDate = DateTimeOffset.Now.AddDays(14).ToUnixTimeMilliseconds();

            TimePickerDialog timeDialog = new TimePickerDialog(view.Context,
                                                                OnTimeSet, 
                                                                currently.Hour, 
                                                                currently.Minute, 
                                                                true);

            timeDialog.Show();
            dateDialog.Show();
            
        }

        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            sportEvent.Date = e.Date;
        }

        public void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            sportEvent.Date = sportEvent.Date.AddHours(e.HourOfDay);
            sportEvent.Date = sportEvent.Date.AddMinutes(e.Minute);
            btnDate.Text = sportEvent.Date.ToString("dd.MM.yyyy H:mm", CultureInfo.CurrentCulture);
        }

        
    }
}
