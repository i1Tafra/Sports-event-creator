using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using Fragment = Android.Support.V4.App.Fragment;

namespace SportsEventCreator.Resources
{
    internal class UserGroupsFragment : Fragment
    {

        #region Properties and variables
        private RecyclerView rwGroups;
        private View view;
        private UserGroupsAdapter adapter;
        #endregion

        private void InitGUIElements(View view)
        {
            rwGroups = view.FindViewById<RecyclerView>(Resource.Id.rw_groups);

            rwGroups.HasFixedSize = true;
            var layoutManager = new LinearLayoutManager(view.Context, LinearLayoutManager.Vertical, false);
            rwGroups.SetLayoutManager(layoutManager);

            adapter = new UserGroupsAdapter(Instance.UserGroups.Groups);
            rwGroups.SetAdapter(adapter);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.fragment_user_groups, container, false);
            InitGUIElements(view);
            InitBtnClickListeners();
            return view;
        }

        private void InitBtnClickListeners()
        {
            adapter.ItemClick += (sender, e) => {
                var extra = e as UserGroupsAdapterClickEventArgs;
                Toast.MakeText(view.Context, Resource.String.warn_implementation_need, ToastLength.Short).Show();
                //TODO: Implement details fragment/ activity for group
                //var activity = new Intent(this, typeof(TransactionActivity));
                //activity.PutExtra("positon", extra.Position);
                //StartActivity(activity);
            };
        }

        //TODO: Implement listener for data changed in user groups
    }
}