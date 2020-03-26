namespace SportsEventCreator.Database
{
    /// <summary>
    /// Defines the <see cref="EventUser" />
    /// </summary>
    public class EventUser : User
    {
        public EventUser()
        {

        }
        /// <summary>
        /// Gets or sets a value indicating whether IsAttending
        /// </summary>
        public bool IsAttending { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventUser"/> class.
        /// </summary>
        /// <param name="user">The user<see cref="User"/></param>
        public EventUser(User user) : base(user)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventUser"/> class.
        /// </summary>
        /// <param name="username">The username<see cref="string"/></param>
        /// <param name="email">The email<see cref="string"/></param>
        /// <param name="isAttending">The isAttending<see cref="bool"/></param>
        public EventUser(string username, string email, bool isAttending = false) : base(username, email)
        {
            IsAttending = isAttending;
        }
    }
}
