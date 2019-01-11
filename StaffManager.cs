using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StaffManager
    {
        public List<Room> Rooms { get; }

        public StaffManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Room";

                Rooms = command.GetDataTable().Select().Select(x => new Room((string)x["RoomID"])).ToList();
            }
        }
    }
}
