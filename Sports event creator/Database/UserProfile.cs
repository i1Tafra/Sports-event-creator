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
    public class UserProfile : User
    {
        public DateTimeOffset DateCreated { get; set; } = DateTime.Now;
        public UserProfile(string username, string email) : base(username, email)
        {
        }
    }
}