using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StudentManager
    {
        public List<User> Students { get; }

        public StudentManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var studentCommand = connection.CreateCommand();

                // select all from user where email ends with student.rmit.edu.au
                studentCommand.CommandText = "select * from [User] where Email like '%_@student%.rmit%.edu%.au%'";

                Students = studentCommand.GetDataTable().Select().Select(x =>
                    new User((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();
            }

        }
    }
}
