using System;
namespace AppointmentSchedulingReservation
{
    public interface IStaff
    {
        void ListStaff();
        void RoomAvailability();
        void CreateSlot();
        void RemoveSlot();
    }

    public interface IStudent
    {
        void ListStudents();
        void StaffAvailability();
        void MakeBooking();
        void CancelBooking();
    }
}
