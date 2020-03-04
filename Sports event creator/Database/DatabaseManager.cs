using Plugin.CloudFirestore;
using System;
using System.Threading.Tasks;

namespace SportsEventCreator.Database
{
    internal static class DatabaseManager
    {
        private const string COLLETCTION_EVENT = "Events";
        private const string COLLECTION_USER = "Users";
        private const string COLLECTION_USER_GROUPS = "UserGroups";

        private const string ATTRIBUTE_USER_GROUPS_CREATOR = "Creator";
        private const string ATTRIBUTE_USER_USERNAME_ = "Username";
        private const string ATTRIBUTE_USER_EMAIL_ = "Email";

        #region user profile
        /// <summary>
        /// Add new User profile to firebase  
        /// </summary>
        /// <param name="user">newly created user</param>
        /// <returns></returns>
        internal static Task AddUser(UserProfile user)
        {
            return CrossCloudFirestore.Current
                          .Instance
                          .GetCollection(COLLECTION_USER)
                          .AddDocumentAsync(user);
        }

        /// <summary>
        /// Update user profile with provided userm param based on document id
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        internal static Task UpdateUser(string documentId, UserProfile user)
        {
            return CrossCloudFirestore.Current
                         .Instance
                         .GetCollection(COLLECTION_USER)
                         .GetDocument(documentId)
                         .UpdateDataAsync(user);
        }

        /// <summary>
        /// Get user based on username or mail address
        /// </summary>
        /// <param name="userID">username or email address to found</param>
        /// <returns></returns>
        internal static Task<IQuerySnapshot> GetUser(string userID)
        {
            IQuery query = CrossCloudFirestore.Current
                            .Instance
                            .GetCollection(COLLECTION_USER)
                            .LimitTo(1);

            if (userID.Contains("@", StringComparison.Ordinal))
            {
                return query.WhereEqualsTo(ATTRIBUTE_USER_EMAIL_, userID)
                    .GetDocumentsAsync();
            }

            return query.WhereEqualsTo(ATTRIBUTE_USER_USERNAME_, userID)
            .GetDocumentsAsync();
        }
        #endregion

        #region user groups
        /// <summary>
        /// Add user groups to firestore for new user without user groups
        /// </summary>
        /// <param name="userGroups">empty user groups</param>
        /// <returns></returns>
        internal static Task AddUserGroups(UserGroups userGroups)
        {
            return CrossCloudFirestore.Current
                          .Instance
                          .GetCollection(COLLECTION_USER_GROUPS)
                          .AddDocumentAsync(userGroups);
        }

        /// <summary>
        /// Update user groups with provided groups param based on document id
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        internal static Task UpdateUserGroups(string documentId, UserGroups groups)
        {
            return CrossCloudFirestore.Current
                         .Instance
                         .GetCollection(COLLECTION_USER_GROUPS)
                         .GetDocument(documentId)
                         .UpdateDataAsync(groups);
        }


        /// <summary>
        /// Get User Groups based on Creator
        /// </summary>
        /// <param name="user">Creator and owner of groups</param>
        /// <returns></returns>
        internal static Task<IQuerySnapshot> GetUserGroups(User user)
        {
            User only_user = new User(user);
            return CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection(COLLECTION_USER_GROUPS)
                                     .WhereEqualsTo(ATTRIBUTE_USER_GROUPS_CREATOR, only_user)
                                     .LimitTo(1)
                                     .GetDocumentsAsync();
        }
        #endregion

        /// <summary>
        /// Add new sport event in firestore
        /// </summary>
        /// <param name="sportEvent">event to be added</param>
        /// <returns></returns>
        internal static Task AddSportEvent(SportEvent sportEvent)
        {
            return CrossCloudFirestore
                .Current
                .Instance
                .GetCollection(COLLETCTION_EVENT)
                .AddDocumentAsync(sportEvent);
        }

    }
}