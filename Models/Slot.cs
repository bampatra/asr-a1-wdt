using System;
namespace AppointmentSchedulingReservation
{
    public class Slot
    {
        public string RoomID { get; }
        public DateTime StartTime { get; }
        public string StaffID { get; }
        public dynamic BookedInStudentID { get; }

        public Slot(string roomID, DateTime startTime, string staffID, dynamic studentID)
        {
            RoomID = roomID;
            StartTime = startTime;
            StaffID = staffID;
            BookedInStudentID = studentID;
        }
    }
}
