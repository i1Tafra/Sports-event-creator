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
    public class User
    {
        public string Username { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;

        public User(string username, string email)
        {
            Username = username;
            Email = email;
        }

        public User(User user)
        {
            if (user == null)
                return;

            this.Username = user.Username;
            this.Email = user.Email;
        }

        public override bool Equals(object obj)
        {
            return (obj is User user) ? Equals(user) : false;
        }

        private bool Equals(User other)
        {
            if (other == null) return false;
            return Username.Equals(other.Username, StringComparison.Ordinal)
                && Email.Equals(other.Email, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode(System.StringComparison.Ordinal) * 17 + Email.GetHashCode(System.StringComparison.Ordinal);
        }
    }
}