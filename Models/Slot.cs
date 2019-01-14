using System;
namespace AppointmentSchedulingReservation
{
    public class Slot
    {
        public string RoomID { get; set; }
        public DateTime StartTime { get; set; }
        public string StaffID { get; set; }
        public dynamic BookedInStudentID { get; set; }

        public Slot(string roomID, DateTime startTime, string staffID, dynamic studentID)
        {
            RoomID = roomID;
            StartTime = startTime;
            StaffID = staffID;
            BookedInStudentID = studentID;
        }
    }
}
