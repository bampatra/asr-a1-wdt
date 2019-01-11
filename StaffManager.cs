using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StaffManager
    {
        public List<Room> Rooms { get; }
        public List<User> Staffs { get; }

        public StaffManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var roomCommand = connection.CreateCommand();
                var staffCommand = connection.CreateCommand();


                roomCommand.CommandText = "select * from Room";
                // select all from user where email ends with rmit.edu.au
                staffCommand.CommandText = "select * from [User] where Email like '%_@rmit%.edu%.au%'";

                Rooms = roomCommand.GetDataTable().Select().Select(x => new Room((string)x["RoomID"])).ToList();
                Staffs = staffCommand.GetDataTable().Select().Select(x => 
                    new User((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();
            }
        }
    }
}
