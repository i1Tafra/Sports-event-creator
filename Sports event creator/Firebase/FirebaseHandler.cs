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
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

namespace SportsEventCreator.Firebase
{
    public class FirebaseHandler
    {
        private FirebaseApp App { get;} 
        public FirebaseAuth Auth { get;}
        public FirebaseFirestore Firestore { get; }

        private FirebaseHandler(Context context)
        {
            App = FirebaseApp.InitializeApp(context);
            Auth = FirebaseAuth.GetInstance(App);
            Firestore = FirebaseFirestore.GetInstance(App);
        }

        public static void Init(Context context)
        {
            _instance = new FirebaseHandler(context);
        }

        private static FirebaseHandler _instance;
        public static FirebaseHandler Instance { get => _instance; } 
    }
}
