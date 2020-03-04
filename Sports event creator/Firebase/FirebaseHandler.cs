using Android.Content;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

namespace SportsEventCreator.Firebase
{
    public class FirebaseHandler
    {
        private FirebaseApp App { get; }
        public FirebaseAuth Auth { get; }
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
        public static FirebaseHandler Instance => _instance;
    }
}
