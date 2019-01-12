using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StudentManager
    {
        public List<User> Students { get; }
        public List<Slot> Slots { get; }

        public StudentManager()
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                var studentCommand = connection.CreateCommand();
                var slotCommand = connection.CreateCommand();

                // select all from user where email ends with student.rmit.edu.au
                studentCommand.CommandText = "select * from [User] where Email like '%_@student%.rmit%.edu%.au%'";
                slotCommand.CommandText = "select * from Slot";

                Students = studentCommand.GetDataTable().Select().Select(x =>
                    new User((string)x["UserID"], (string)x["Name"], (string)x["Email"])).ToList();

                Slots = slotCommand.GetDataTable().Select().Select(x =>
                    new Slot((string)x["RoomID"], (DateTime)x["StartTime"], (string)x["StaffID"],
                             (dynamic)x["BookedInStudentID"])).ToList();
            }

        }

        public Slot GetSlot(string roomName, DateTime date) => Slots.FirstOrDefault(x => 
        x.RoomID.Equals(roomName) && x.StartTime == date);

        public void MakeBooking(Slot slot, string StudentID)
        {
            using (var connection = Program.ConnectionString.CreateConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "update Slot set BookedInStudentId = @studentID where " +
                    "RoomID = @roomID and StartTime = @starttime";
                command.Parameters.AddWithValue("studentID", StudentID);
                command.Parameters.AddWithValue("roomID", slot.RoomID);
                command.Parameters.AddWithValue("starttime", slot.StartTime);

                command.ExecuteNonQuery();
            }
        }
    }
}
