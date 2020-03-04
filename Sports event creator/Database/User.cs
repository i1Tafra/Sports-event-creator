using System;

namespace SportsEventCreator.Database
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public User(string username, string email)
        {
            Username = username;
            Email = email;
        }

        public User(User user)
        {
            if (user == null)
            {
                return;
            }

            Username = user.Username;
            Email = user.Email;
        }

        /// <summary>
        /// Used by firestore
        /// </summary>
        public User() { }

        public override bool Equals(object obj)
        {
            return (obj is User user) ? Equals(user) : false;
        }

        private bool Equals(User other)
        {
            if (other == null)
            {
                return false;
            }

            return Username.Equals(other.Username, StringComparison.Ordinal)
                && Email.Equals(other.Email, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode(System.StringComparison.Ordinal) * 17 + Email.GetHashCode(System.StringComparison.Ordinal);
        }
    }
}