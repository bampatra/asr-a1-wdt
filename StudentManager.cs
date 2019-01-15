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

        //private static StaffManager StaffManager { get; } = new StaffManager();

        public StudentManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var studentCommand = connection.CreateCommand();
                //var slotCommand = connection.CreateCommand();

                // select all from user where email ends with student.rmit.edu.au
                studentCommand.CommandText = "select * from [User] where Email like '%_@student%.rmit%.edu%.au%'";
                //slotCommand.CommandText = "select * from Slot";

                Students = studentCommand.GetDataTable().Select().Select(x =>
                    new User((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();

                //Slots = slotCommand.GetDataTable().Select().Select(x =>
                //new Slot((string)x["RoomID"], (DateTime)x["StartTime"], (string)x["StaffID"],
                //(dynamic)x["BookedInStudentID"])).ToList();

                //Slots = StaffManager.UserManager.Slots;

                Slots = UserManager.Instance.Slots;
            }

        }

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



        public Slot GetSlot(string roomName, DateTime date) => Slots.FirstOrDefault(x => 
        x.RoomID.Equals(roomName) && x.StartTime == date);

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
                        var command = connection.CreateCommand();
                        command.CommandText =
                            "update Slot set BookedInStudentId = @studentID where " +
                            "RoomID = @roomID and StartTime = @starttime";
                        command.Parameters.AddWithValue("studentID", StudentID);
                        command.Parameters.AddWithValue("roomID", slot.RoomID);
                        command.Parameters.AddWithValue("starttime", slot.StartTime);
                        command.ExecuteNonQuery();

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

        public void CancelBooking(Slot slot)
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {

                if (slot.BookedInStudentID is DBNull)
                {
                    Console.WriteLine("No booking is found for this slot");
                }
                else
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                        "update Slot set BookedInStudentId = null where " +
                        "RoomID = @roomID and StartTime = @starttime";
                    command.Parameters.AddWithValue("roomID", slot.RoomID);
                    command.Parameters.AddWithValue("starttime", slot.StartTime);
                    command.ExecuteNonQuery();

                    slot.BookedInStudentID = null;
                    Console.WriteLine("Booking has been cancelled");
                }

            }
        }

        public bool CheckMaxBooking(string date, string StudentID, string RoomID)
        {
            int countStudentBookings = 0;
            string[] dateParts = date.Split('-');

            // create new date from the parts
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
