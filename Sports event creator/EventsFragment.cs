//using Android.Gms.Tasks;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SportsEventCreator.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using Fragment = Android.Support.V4.App.Fragment;

namespace SportsEventCreator.Resources
{
    internal class EventsFragment : Fragment
    {

        #region Properties and variables
        private RecyclerView rwEvents;
        private View view;
        private EventAdapter adapter;
        #endregion

        private void InitGUIElements(View view)
        {
            rwEvents = view.FindViewById<RecyclerView>(Resource.Id.rw_events);

            rwEvents.HasFixedSize = true;
            var layoutManager = new LinearLayoutManager(view.Context, LinearLayoutManager.Vertical, false);
            rwEvents.SetLayoutManager(layoutManager);

            adapter = new EventAdapter(Instance.Events);
            rwEvents.SetAdapter(adapter);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            LoadFromDatabase();
            view = inflater.Inflate(Resource.Layout.fragment_events, container, false);
            InitGUIElements(view);
            InitBtnClickListeners();
            return view;
        }

        private async void LoadFromDatabase()
        {
            if (Instance.Events.Count > 0)
                return;

            //TODO: This will add only if user is not attending
            var snapshot = await DatabaseManager.GetSportEvents(Instance.User).ConfigureAwait(true);
            Instance.Events.AddRange(snapshot.ToObjects<SportEvent>().ToList());
            snapshot = await DatabaseManager.GetSportEvents(Instance.User, is_attending: true).ConfigureAwait(true);
            Instance.Events.AddRange(snapshot.ToObjects<SportEvent>().ToList());
            rwEvents.GetAdapter().NotifyDataSetChanged();
        }

        private void InitBtnClickListeners()
        {
            adapter.ItemClick += (sender, e) => {
                var extra = e as EventAdapterClickEventArgs;
                Toast.MakeText(view.Context, Resource.String.warn_implementation_need, ToastLength.Short).Show();
                //TODO: Implement details fragment/ activity for event
                //var activity = new Intent(this, typeof(TransactionActivity));
                //activity.PutExtra("positon", extra.Position);
                //StartActivity(activity);
            };
        }

        //TODO: Implement listener for data changed in user groups
    }
}