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
    public class UserGroups
    {
        public User Creator { get; set; }

        private readonly List<Group> _groups = new List<Group>();
        public List<Group> Groups { get => _groups; }

        public void AddGroup(Group group)
        {
            if (ValidateGroupUnique(group))
                _groups.Add(group);
        }

        private bool ValidateGroupUnique(Group group)
        {
            return !Groups.Exists(g => g.Name.Equals(group.Name, StringComparison.Ordinal));
        }
    }
}