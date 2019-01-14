using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class UserManager
    {
        public List<Slot> Slots { get; set; }

        public UserManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var slotCommand = connection.CreateCommand();
                slotCommand.CommandText = "select * from Slot";
                Slots = slotCommand.GetDataTable().Select().Select(x =>
                    new Slot((string)x["RoomID"], (DateTime)x["StartTime"], (string)x["StaffID"],
                             (dynamic)x["BookedInStudentID"])).ToList();

            }
        }
    }
}
