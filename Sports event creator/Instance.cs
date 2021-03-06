﻿using Plugin.CloudFirestore;
using SportsEventCreator.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsEventCreator
{
    internal static class Instance
    {
        internal static UserProfile User { get; set; }
        internal static UserGroups UserGroups { get; set; }

        internal static List<SportEvent> Events { get; set; } = new List<SportEvent>();

        /// <summary>
        /// Load User data from firestore. 
        /// Loaded items are user profile and user groups, as wll as documents Ids for loaded documents
        /// </summary>
        /// <param name="email">email address</param>
        internal static async Task<IQuerySnapshot> LoadUserData(string email)
        {
            var task = DatabaseManager.GetUser(email);
            Plugin.CloudFirestore.IQuerySnapshot snapshot = await task.ConfigureAwait(false);

            Instance.User = snapshot.ToObjects<UserProfile>().FirstOrDefault();

            snapshot = await DatabaseManager.GetUserGroups(User)
                .ConfigureAwait(false);

            Instance.UserGroups = snapshot.ToObjects<UserGroups>().FirstOrDefault();

            //TODO: This will add only if user is not attending
            //snapshot = await DatabaseManager.GetSportEvents(User)
            //    .ConfigureAwait(false);

            //Instance.Events = snapshot.ToObjects<SportEvent>().ToList();
            return snapshot;
    }

        /// <summary>
        /// Initialize newly created user in firestore. 
        /// Save user profile and empty user group in firestore
        /// </summary>
        /// <param name="user"></param>
        /// <returns>2 Tasks for creating user and usergroups</returns>
        internal static List<Task> InitFirestoreUser(string username, string email)
        {
            List<Task> tasks = new List<Task>
            {
                DatabaseManager.AddUser(new UserProfile(username, email)),
                DatabaseManager.AddUserGroups(new UserGroups(username, email))
            };

            return tasks;
        }
    }
}