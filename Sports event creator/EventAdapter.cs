using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using SportsEventCreator.Database;
using System.Collections.Generic;
using System.Globalization;

namespace SportsEventCreator
{
    class EventAdapter : RecyclerView.Adapter
    {
        public event EventHandler<EventAdapterClickEventArgs> ItemClick;
        public event EventHandler<EventAdapterClickEventArgs> ItemLongClick;
        List<SportEvent> items;

        public EventAdapter(List<SportEvent> events)
        {
            items = events;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater
                .From(parent.Context)
                .Inflate(Resource.Layout.item_event, parent, false);

            var vh = new EventAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            var holder = viewHolder as EventAdapterViewHolder;
            holder.Date.Text = item.Date.ToString("G", CultureInfo.CurrentCulture);
            holder.Location.Text = item.Location;
            holder.UserCount.Text = item.Users.Count.ToString("G", CultureInfo.CurrentCulture);
        }

        public override int ItemCount => items.Count;

        void OnClick(EventAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(EventAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class EventAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Date { get; set; }
        public TextView Location { get; set; }
        public TextView UserCount { get; set; }

        public EventAdapterViewHolder(View itemView, Action<EventAdapterClickEventArgs> clickListener,
                            Action<EventAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            Date = itemView.FindViewById<TextView>(Resource.Id.txt_date);
            Location = itemView.FindViewById<TextView>(Resource.Id.txt_location);
            UserCount = itemView.FindViewById<TextView>(Resource.Id.txt_users_count);

            itemView.Click += (sender, e) => clickListener(new EventAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new EventAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class EventAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}