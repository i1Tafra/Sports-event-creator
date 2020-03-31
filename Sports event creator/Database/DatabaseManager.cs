using Android.Test;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsEventCreator.Database
{
    internal static class DatabaseManager
    {
        private const string COLLETCTION_EVENT = "Events";
        private const string COLLECTION_USER = "Users";
        private const string COLLECTION_USER_GROUPS = "UserGroups";

        private const string ATTRIBUTE_USER_GROUPS_CREATOR = "Creator";
        private const string ATTRIBUTE_USER_USERNAME = "Username";
        private const string ATTRIBUTE_USER_EMAIL_ = "Email";
        private const string ATTRIBUTE_USERS = "Users";
        private const string ATTRIBUTE_DATE = "Date";
        private const string ATTRIBUTE_LOCATION = "Location";

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
        /// <param name="groups"></param>
        /// <returns></returns>
        internal static Task UpdateUser( UserProfile user)
        {
            return CrossCloudFirestore.Current
                         .Instance
                         .GetCollection(COLLECTION_USER)
                         .GetDocument(user.DocumentId)
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

            return query.WhereEqualsTo(ATTRIBUTE_USER_USERNAME, userID)
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
        /// <param name="groups"></param>
        /// <returns></returns>
        internal static Task UpdateUserGroups(UserGroups groups)
        {
            return CrossCloudFirestore.Current
                         .Instance
                         .GetCollection(COLLECTION_USER_GROUPS)
                         .GetDocument(groups.DocumentId)
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

        #region Events
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

        /// <summary>
        /// Update sport event with provided sport event param based on document id
        /// </summary>
        /// <param name="sportEvent"></param>
        /// <returns></returns>
        internal static Task UpdateSportEvent(SportEvent sportEvent)
        {
            return CrossCloudFirestore.Current
                         .Instance
                         .GetCollection(COLLETCTION_EVENT)
                         .GetDocument(sportEvent.DocumentId)
                         .UpdateDataAsync(sportEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal static Task<IQuerySnapshot> GetSportEvents(User user, bool is_attending = false)
        {
            EventUser only_user = new EventUser(user);
            only_user.IsAttending = is_attending;

            return CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection(COLLETCTION_EVENT)
                                     .WhereGreaterThan(ATTRIBUTE_DATE, DateTime.Now)
                                     .WhereArrayContains(ATTRIBUTE_USERS, only_user)
                                     .LimitTo(5)
                                     .OrderBy(ATTRIBUTE_DATE)
                                     .GetDocumentsAsync();
        }

        internal static Task<IQuerySnapshot> GetSportEvents(SportEvent sportEvent)
        {
            return CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection(COLLETCTION_EVENT)
                                     .WhereEqualsTo(ATTRIBUTE_USER_GROUPS_CREATOR, sportEvent.Creator)
                                     .WhereEqualsTo(ATTRIBUTE_DATE, sportEvent.Date)
                                     .WhereEqualsTo(ATTRIBUTE_LOCATION, sportEvent.Location)
                                     .LimitTo(1)
                                     .GetDocumentsAsync();
        }
        #endregion
    }
}