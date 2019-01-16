using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StaffManager
    {
        private static StaffManager instance = null;

        public List<Room> Rooms { get; }
        public List<User> Staffs { get; }


        public StaffManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var roomCommand = connection.CreateCommand();
                var staffCommand = connection.CreateCommand();


                roomCommand.CommandText = "select * from Room";

                // Get all users whose email ends with rmit.edu.au
                staffCommand.CommandText = "select * from [User] where Email like '%_@rmit%.edu%.au%'";

                // Create new objects from data gathered and add them to generic List<T>
                Rooms = roomCommand.GetDataTable().Select().Select(x => new Room((string)x["RoomID"])).ToList();
                Staffs = staffCommand.GetDataTable().Select().Select(x =>
                    new User((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();
                    
            }
        }

        // Get the instance of the object
        public static StaffManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StaffManager();
                }
                return instance;
            }

        }

        // Create a new slot 
        public void CreateSlot(string RoomName, string Date, string Time, string StaffID)
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                connection.Open();

                // Convert date and time into one datetime object
                string[] dateParts = Date.Split('-');
                string[] timeParts = Time.Split(':');
                DateTime newDate = new
                    DateTime(Convert.ToInt32(dateParts[2]),
                    Convert.ToInt32(dateParts[1]),
                    Convert.ToInt32(dateParts[0]),
                    Convert.ToInt32(timeParts[0]),
                    Convert.ToInt32(timeParts[1]),
                    0);
                    

                // Check maximum booking
                if(CheckMaxSlot(Date, StaffID, RoomName) == true)
                {
                    try
                    {
                        // Add new slot to database
                        var command = connection.CreateCommand();
                        command.CommandText = $"insert into Slot (RoomID, StartTime, StaffID, BookedInStudentID) " +
                            "values(@roomID, @starttime, @staffID, null)";
                        command.Parameters.AddWithValue("roomID", RoomName);
                        command.Parameters.AddWithValue("starttime", newDate);
                        command.Parameters.AddWithValue("staffID", StaffID);
                        command.ExecuteNonQuery();

                        // Add new slot to local memory
                        UserManager.Instance.Slots.Add(new Slot(RoomName, newDate, StaffID, null));

                        Console.WriteLine("Slot created successfully");
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        Console.WriteLine("Room name or Staff ID does not exist in database.");
                    }

                }
                else
                {
                    Console.WriteLine("Maximum limit has been reached. Unable to create slot.");
                }


            }
        }

        // Remove an existing slot
        public bool RemoveSlot(string RoomName, string Date, string Time)
        {

            bool match = false;
            bool booked = true;
            int index = 0;

            // Convert date and time into one datetime object
            string[] dateParts = Date.Split('-');
            string[] timeParts = Time.Split(':');
            DateTime newDate = new
                DateTime(Convert.ToInt32(dateParts[2]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[0]),
                Convert.ToInt32(timeParts[0]),
                Convert.ToInt32(timeParts[1]),
                0);

            if (!UserManager.Instance.Slots.Any())
            {
                Console.WriteLine("There is no slot in database");
                return false;
            }

            foreach (var x in UserManager.Instance.Slots)
            {
                if (x.RoomID == RoomName && x.StartTime == newDate)
                {
                    using (var connection = Program.ConnectionString.CreateConnection())
                    {
                        // If the slot is not booked
                        if (x.BookedInStudentID is DBNull || x.BookedInStudentID is null)
                        {
                            // Remove slot from database
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText = $"delete from Slot where RoomID = @roomID and " +
                                                    "StartTime = @starttime";
                            command.Parameters.AddWithValue("roomID", RoomName);
                            command.Parameters.AddWithValue("starttime", newDate);
                            command.ExecuteNonQuery();

                            // Remove slot from local memory
                            UserManager.Instance.Slots.RemoveAt(index);
                            match = true;
                            booked = false;
                            return true;

                        }

                    }
                }
                index += 1;
            }

            if (match == false || booked == true)
            {
                Console.WriteLine("Unable to remove slot");
            }

            return false;
        }

        // Check the maximum limit of number of bookings for a room
        public bool CheckMaxSlot(string date, string StaffID, string RoomID)
        {

            int countStaffBookings = 0;
            int countRoomBookings = 0;
            string[] dateParts = date.Split('-');

            DateTime FromDate = new
                DateTime(Convert.ToInt32(dateParts[2]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[0]));

            DateTime ToDate = new
                DateTime(Convert.ToInt32(dateParts[2]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[0]),
                23, 59, 59);


            foreach (var x in UserManager.Instance.Slots)
            {
                // Checks if the staff has not made more than 4 bookings in one day
                if (FromDate <= x.StartTime && x.StartTime <= ToDate && x.StaffID == StaffID)
                {
                    countStaffBookings += 1;
                }

                // Checks if the room has not been booked more than 2 times in one day
                if (FromDate <= x.StartTime && x.StartTime <= ToDate && x.RoomID == RoomID)
                {
                    countRoomBookings += 1;
                }

            }

            if (countStaffBookings < 4 && countRoomBookings < 2)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Print available rooms to the console based on user input
        public void DisplayAvailableRoom(string dateInput)
        {
            string[] dateParts = dateInput.Split('-');
            int RoomCount = 0;

            DateTime FromDate = new
                DateTime(Convert.ToInt32(dateParts[2]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[0]));

            DateTime ToDate = new
                DateTime(Convert.ToInt32(dateParts[2]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[0]),
                23, 59, 59);

            foreach(var x in Rooms)
            {
                foreach(var y in UserManager.Instance.Slots)
                {
                    // Checks the number of bookings that has been made for each room
                    // on the specified date
                    if(FromDate <= y.StartTime && y.StartTime <= ToDate && x.RoomID == y.RoomID)
                    {
                        RoomCount += 1;
                    }
                }

                if (RoomCount < 2) { Console.WriteLine( x.RoomID ); }
                RoomCount = 0;

            }
        }

    }
}
