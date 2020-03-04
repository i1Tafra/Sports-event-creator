using System;

namespace SportsEventCreator.Database
{
    public class UserProfile : User
    {
        public DateTimeOffset DateCreated { get; set; } = DateTime.Now;
        public UserProfile(string username, string email) : base(username, email)
        {
        }

        /// <summary>
        /// Used by firestore
        /// </summary>
        public UserProfile() { }
    }
}