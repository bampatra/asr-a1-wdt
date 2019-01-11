using System;
namespace AppointmentSchedulingReservation
{
    public class User
    {
        public string UserID { get; }
        public string Name { get; }
        public string Email { get; }

        public User(string userID, string name, string email)
        {
            UserID = userID;
            Name = name;
            Email = email;
        }
    }
}
