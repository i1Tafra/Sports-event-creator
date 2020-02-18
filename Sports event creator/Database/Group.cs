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
    public class Group
    {
        public string Name { get; set; }

        private readonly List<User> _users = new List<User>();
        public List<User> Users { get => _users; }

        public void AddUser(User user)
        {
            if (ValidateUserUnique(user))
                _users.Add(user);
        }

        private bool ValidateUserUnique(User user)
        {
            return !Users.Contains(user);
        }
    }
}