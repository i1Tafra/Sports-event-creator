using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using SportsEventCreator.Database;
using System.Collections.Generic;
using System.Globalization;

namespace SportsEventCreator
{
    class UserGroupsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<UserGroupsAdapterClickEventArgs> ItemClick;
        public event EventHandler<UserGroupsAdapterClickEventArgs> ItemLongClick;
        List<Group> items;

        public UserGroupsAdapter(List<Group> groups)
        {
            items = groups;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater
                .From(parent.Context)
                .Inflate(Resource.Layout.item_group, parent, false);

            return new UserGroupsAdapterViewHolder(itemView, OnClick, OnLongClick);
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as UserGroupsAdapterViewHolder;
            holder.Name.Text = item.Name;
            holder.UserCount.Text = item.Users.Count.ToString("G", CultureInfo.CurrentCulture);
        }

        public override int ItemCount => items.Count;

        void OnClick(UserGroupsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(UserGroupsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class UserGroupsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Name { get; set; }
        public TextView UserCount { get; set; }


        public UserGroupsAdapterViewHolder(View itemView, Action<UserGroupsAdapterClickEventArgs> clickListener,
                            Action<UserGroupsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.txt_name);
            UserCount = itemView.FindViewById<TextView>(Resource.Id.txt_users_count);

            itemView.Click += (sender, e) => clickListener(new UserGroupsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new UserGroupsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class UserGroupsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}