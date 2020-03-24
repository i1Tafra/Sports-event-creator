using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;

namespace SportsEventCreator.Database
{
    public enum SportType
    {
        Football,
        Tennis,
        Basketball
    };

    public class SportEvent
    {
        [Id]
        public string DocumentId { get; set; }
        public User Creator { get; set; }
        public string Location { get; set; }
        public DateTimeOffset Date { get; set; }
        public string EventTypeName { get; set; }
        //TODO: This is not possible to write in firestore, reimplement
        [Ignored]
        internal SportType EventType 
        {
           get => ParseSportType(EventTypeName);
           set => EventTypeName = value.ToString(); 
        }

        private readonly List<EventUser> _users = new List<EventUser>();
        public List<EventUser> Users => _users;

        public void AddUser(User user)
        {
            EventUser eventUser = new EventUser(user);
            if (ValidateUserUnique(eventUser))
                _users.Add(eventUser);
        }

        private bool ValidateUserUnique(EventUser user)
        {
            return !Users.Contains(user);
        }

        private static SportType ParseSportType(string value)
        {
            return (SportType)Enum.Parse(typeof(SportType), value, true);
        }
    }
}