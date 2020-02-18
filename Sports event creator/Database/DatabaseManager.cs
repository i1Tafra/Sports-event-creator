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
using Plugin.CloudFirestore;
using System.Threading.Tasks;

namespace SportsEventCreator.Database
{
    public static class DatabaseManager
    {
        private const string EVENT_COLECTION = "Events";
        private const string USER_COLECTION = "Users";

        public static Task AddSportEvent(SportEvent sportEvent)
        {
           return CrossCloudFirestore.Current
                         .Instance
                         .GetCollection(EVENT_COLECTION)
                         .AddDocumentAsync(sportEvent);
        }

        public static Task AddUser(UserProfile user)
        {
            return CrossCloudFirestore.Current
                          .Instance
                          .GetCollection(USER_COLECTION)
                          .AddDocumentAsync(user);
        }
    }
}