using System;
using System.Collections.Generic;

namespace SportsEventCreator.Database
{
    public class UserGroups
    {
        /// <summary>
        /// Used by firestore
        /// </summary>
        public UserGroups() { }

        public UserGroups(string username, string email)
        {
            Creator = new User(username, email);
        }

        public User Creator { get; set; }

        public List<Group> Groups { get; internal set; } = new List<Group>();

        internal void AddGroup(Group group)
        {
            if (ValidateGroupUnique(group))
                Groups.Add(group);
        }

        private bool ValidateGroupUnique(Group group)
        {
            return !Groups.Exists(g => g.Name.Equals(group.Name, StringComparison.Ordinal));
        }
    }
}