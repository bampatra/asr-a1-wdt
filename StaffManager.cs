using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StaffManager
    {
        public List<Room> Rooms { get; }
        public List<User> Staffs { get; }
        public List<Slot> Slots { get; }


        public StaffManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var roomCommand = connection.CreateCommand();
                var staffCommand = connection.CreateCommand();
                var slotCommand = connection.CreateCommand();


                roomCommand.CommandText = "select * from Room";
                // select all from user where email ends with rmit.edu.au
                staffCommand.CommandText = "select * from [User] where Email like '%_@rmit%.edu%.au%'";
                slotCommand.CommandText = "select * from Slot";

                Rooms = roomCommand.GetDataTable().Select().Select(x => new Room((string)x["RoomID"])).ToList();
                Staffs = staffCommand.GetDataTable().Select().Select(x => 
                    new User((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();
                Slots = slotCommand.GetDataTable().Select().Select(x =>
                    new Slot((string)x["RoomID"], (DateTime)x["StartTime"], (string)x["StaffID"],
                             (dynamic)x["BookedInStudentID"])).ToList();

            }
        }

        public void CreateSlot(string RoomName, string Date, string Time, string StaffID)
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                connection.Open();

                // Convert date and time into one datetime object
                string[] dateParts = Date.Split('-');
                string[] timeParts = Time.Split(':');

                // create new date from the parts
                DateTime newDate = new
                    DateTime(Convert.ToInt32(dateParts[2]),
                    Convert.ToInt32(dateParts[1]),
                    Convert.ToInt32(dateParts[0]),
                    Convert.ToInt32(timeParts[0]),
                    Convert.ToInt32(timeParts[1]),
                    0);

                // Check if room and StaffID exists in database

                // Check maximum booking

                // Add to database
                var command = connection.CreateCommand();

                command.CommandText = $"insert into Slot (RoomID, StartTime, StaffID, BookedInStudentID) " +
                    "values(@roomID, @starttime, @staffID, null)";
                command.Parameters.AddWithValue("roomID", RoomName);
                command.Parameters.AddWithValue("starttime", newDate);
                command.Parameters.AddWithValue("staffID", StaffID);

                command.ExecuteNonQuery();
            }
        }

        public bool RemoveSlot(string RoomName, string Date, string Time)
        {

            bool match = false;
            // Convert date and time into one datetime object
            string[] dateParts = Date.Split('-');
            string[] timeParts = Time.Split(':');

            // create new date from the parts
            DateTime newDate = new
                DateTime(Convert.ToInt32(dateParts[2]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[0]),
                Convert.ToInt32(timeParts[0]),
                Convert.ToInt32(timeParts[1]),
                0);

            if (!Slots.Any())
            {
                Console.WriteLine("There is no slot in database");
                return false;
            }

            foreach (var x in Slots)
            {
                if (x.RoomID == RoomName && x.StartTime == newDate)
                {
                    using (var connection = Program.ConnectionString.CreateConnection())
                    {
                        connection.Open();

                        var command = connection.CreateCommand();

                        command.CommandText = $"delete from Slot where RoomID = @roomID and " +
                                                "StartTime = @starttime";
                        command.Parameters.AddWithValue("roomID", RoomName);
                        command.Parameters.AddWithValue("starttime", newDate);

                        command.ExecuteNonQuery();
                        match = true;
                        return true;

                    }
                }
            }

            if (match == false)
            {
                Console.WriteLine("Slot is not found");
            }

            return false;
        }

    }
}
