using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SportsEventCreator.Database
{
    public class EventUser : User
    {

        public bool IsAttending { get; set; } = false;

        public EventUser(User user) : base(user)
        {
        }

        public EventUser(string username, string email, bool isAttending = false) : base(username, email)
        {
            IsAttending = isAttending;
        }

    }
}