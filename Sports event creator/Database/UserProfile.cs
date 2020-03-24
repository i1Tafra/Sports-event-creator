using Plugin.CloudFirestore.Attributes;
using System;

namespace SportsEventCreator.Database
{
    public class UserProfile : User
    {
        [Id]
        public string Id { get; set; }
        [ServerTimestamp(CanReplace = false)]
        public DateTimeOffset DateCreated { get; set; }
        [ServerTimestamp]
        public DateTimeOffset LastLogin { get; set; }
        public UserProfile(string username, string email) : base(username, email)
        {
        }

        /// <summary>
        /// Used by firestore
        /// </summary>
        public UserProfile() { }
    }
}