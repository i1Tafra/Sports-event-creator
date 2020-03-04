using SportsEventCreator.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsEventCreator
{
    internal static class Instance
    {
        internal static UserProfile User { get; set; }
        internal static string UserDocumentId { get; set; }
        internal static UserGroups UserGroups { get; set; }
        internal static string UserGroupsDocumentId { get; set; }

        /// <summary>
        /// Load User data from firestore. 
        /// Loaded items are user profile and user groups, as wll as documents Ids for loaded documents
        /// </summary>
        /// <param name="username"></param>
        internal static async void LoadUserData(string username)
        {
            Plugin.CloudFirestore.IQuerySnapshot snapshot = await DatabaseManager.GetUser(username)
                .ConfigureAwait(false);

            Instance.User = snapshot.ToObjects<UserProfile>().FirstOrDefault();
            Instance.UserDocumentId = snapshot.Documents.FirstOrDefault()?.Id;

            snapshot = await DatabaseManager.GetUserGroups(User)
                .ConfigureAwait(false);

            Instance.UserGroups = snapshot.ToObjects<UserGroups>().FirstOrDefault();
            Instance.UserGroupsDocumentId = snapshot.Documents.FirstOrDefault()?.Id;
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