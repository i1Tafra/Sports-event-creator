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
    public enum SportType
    {
        Football,
        Tenis,
        Basketball
    };

    public class SportEvent
    {
        public User Creator { get; set; }
        public string Location { get; set; }
        public DateTimeOffset Date { get; set; }
        public SportType EventType { get; set; }

        private readonly List<EventUser> _users = new List<EventUser>();
        public List<EventUser> Users { get => _users; }

        public void AddUser(User user)
        {
            var eventUser = new EventUser(user);
            if (ValidateUserUnique(eventUser))
                _users.Add(eventUser);
        }

        private bool ValidateUserUnique(EventUser user)
        {
            return !Users.Contains(user);
        }
    }
}