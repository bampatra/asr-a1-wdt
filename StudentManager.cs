using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StudentManager
    {
        private static StudentManager instance = null;

        public List<User> Students { get; }
        public List<Slot> Slots { get;  }


        public StudentManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var studentCommand = connection.CreateCommand();

                // Get all users whose email ends with student.rmit.edu.au
                studentCommand.CommandText = "select * from [User] where Email like '%_@student%.rmit%.edu%.au%'";

                // Create new objects from data gathered and add them to generic List<T>
                Students = studentCommand.GetDataTable().Select().Select(x =>
                    new User((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();

                Slots = UserManager.Instance.Slots;
            }

        }

        // Get the instance of the object
        public static StudentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentManager();
                }
                return instance;
            }

        }

        // Get a slot object that matches the given criteria
        public Slot GetSlot(string roomName, DateTime date) => Slots.FirstOrDefault(x => 
        x.RoomID.Equals(roomName) && x.StartTime == date);

        // Book an available slot
        public void MakeBooking(Slot slot, string StudentID)
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                // Checks if the slot has not been booked
                if(slot.BookedInStudentID is DBNull || slot.BookedInStudentID is null)
                {
                    connection.Open();

                    try
                    {
                        // Update slot in database
                        var command = connection.CreateCommand();
                        command.CommandText =
                            "update Slot set BookedInStudentId = @studentID where " +
                            "RoomID = @roomID and StartTime = @starttime";
                        command.Parameters.AddWithValue("studentID", StudentID);
                        command.Parameters.AddWithValue("roomID", slot.RoomID);
                        command.Parameters.AddWithValue("starttime", slot.StartTime);
                        command.ExecuteNonQuery();

                        // Update slot in local memory
                        slot.BookedInStudentID = StudentID;
                        Console.WriteLine("Slot has been booked");
                    }

                    catch (System.Data.SqlClient.SqlException)
                    {
                        Console.WriteLine("Room name or Staff ID does not exist in database.");
                    }
                }
                else
                {
                    Console.WriteLine("Slot is already booked. Please choose another slot.");
                }

            }
        }

        // Cancel a booked slot
        public void CancelBooking(Slot slot)
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {

                // Check if the slot is currently booked or not
                if (slot.BookedInStudentID is DBNull)
                {
                    Console.WriteLine("No booking is found for this slot");
                }
                else
                {
                    connection.Open();

                    // Update slot in database
                    var command = connection.CreateCommand();
                    command.CommandText =
                        "update Slot set BookedInStudentId = null where " +
                        "RoomID = @roomID and StartTime = @starttime";
                    command.Parameters.AddWithValue("roomID", slot.RoomID);
                    command.Parameters.AddWithValue("starttime", slot.StartTime);
                    command.ExecuteNonQuery();

                    // Update slot in local memory
                    slot.BookedInStudentID = null;
                    Console.WriteLine("Booking has been cancelled");
                }

            }
        }

        // Check the maximum limit of student bookings
        public bool CheckMaxBooking(string date, string StudentID, string RoomID)
        {
            int countStudentBookings = 0;
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

            foreach (var x in Slots)
            {
                if (FromDate <= x.StartTime && x.StartTime <= ToDate)
                {
                    if (x.BookedInStudentID is string)
                    {
                        // Checks the number of booking made by the specified student
                        if (x.BookedInStudentID == StudentID) { countStudentBookings += 1; }
                    } 

                }
            }

            if (countStudentBookings < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
