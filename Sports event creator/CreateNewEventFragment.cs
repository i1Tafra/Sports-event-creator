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

namespace SportsEventCreator.Resources
{
    internal class CreateNewEventFragment : Fragment
    {
        #region Properties and variables
        private SportEvent sportEvent;
        private View view;
        private Button btnDate;
        private Spinner spSports;
        #endregion
        private void InitGUIElements(View view)
        {
            spSports = view.FindViewById<Spinner>(Resource.Id.spinner_sports);
            var sportTypes = SportType.GetValues(typeof(SportType));
            var sportTypesArray = sportTypes.Cast<SportType>().Select(e => e.ToString()).ToArray();
            var adapter = new ArrayAdapter<string>(view.Context, Android.Resource.Layout.SimpleSpinnerItem, sportTypesArray);
            spSports.Adapter = adapter;
            btnDate = view.FindViewById<Button>(Resource.Id.btn_date);

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

            spSports.ItemSelected += (sender, e) =>
            {
                string item = (string)spSports.GetItemAtPosition(e.Position);
                Enum.TryParse(item, out SportType type);
                sportEvent.EventType = type;

            };
        }

        public void ShowDateTimeDialog()
        {

            DateTime currently = DateTime.Now;
            DatePickerDialog dateDialog = new DatePickerDialog(view.Context,
                                                               OnDateSet,
                                                               currently.Year,
                                                               currently.Month - 1,
                                                               currently.Day);

            dateDialog.DatePicker.MinDate = currently.Millisecond;
            

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
            btnDate.Text = sportEvent.Date.ToString();
        }

        public void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            sportEvent.Date = sportEvent.Date.AddHours(e.HourOfDay);
            sportEvent.Date = sportEvent.Date.AddMinutes(e.Minute);
            btnDate.Text = sportEvent.Date.ToString();
        }

        
    }
}
