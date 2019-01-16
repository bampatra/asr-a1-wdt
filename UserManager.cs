using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class UserManager
    {
        private static UserManager instance = null;
        public List<Slot> Slots { get; set; }

        public UserManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var slotCommand = connection.CreateCommand();

                // Get all slots from database
                slotCommand.CommandText = "select * from Slot";

                // Store gathered data in generic List<T>
                Slots = slotCommand.GetDataTable().Select().Select(x =>
                    new Slot((string)x["RoomID"], (DateTime)x["StartTime"], (string)x["StaffID"],
                             (dynamic)x["BookedInStudentID"])).ToList();

            }
        }

        // Get the instance of the object
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserManager();
                }
                return instance;
            }

        }
    }
}
