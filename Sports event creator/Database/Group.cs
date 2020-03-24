using System.Collections.Generic;

namespace SportsEventCreator.Database
{
    public class Group
    {
        public string Name { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        internal void AddUser(User user)
        {
            if (ValidateUserUnique(user))
                Users.Add(new User(user));
        }

        private bool ValidateUserUnique(User user)
        {
            return !Users.Contains(user);
        }

        public override string ToString()
        {
            return $"{Name} [{Users.Count}]";
        }
    }
}