using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class MainMenu
    {
        private StaffManager StaffManager { get; } = new StaffManager();

        public void ListRooms()
        {

            Console.WriteLine("--- List rooms ---");
            if (!StaffManager.Rooms.Any())
            {
                Console.WriteLine("No items present.");
                Console.WriteLine();
                return;
            }

            DisplayRooms(StaffManager.Rooms);

        }

        private void DisplayRooms(IEnumerable<Room> rooms)
        {
            //const string format = "{0,-5}{1,-25}{2}";
            Console.WriteLine("Room name");
            foreach (var x in rooms)
            {
                Console.WriteLine(x.RoomID);
            }
            Console.WriteLine();
        }

        public void ListSlots()
        {
            bool repeat = true;
            while (repeat == true)
            {
                Console.Write("Enter date for slots (dd-mm-yyyy): ");
                string dateInput = Console.ReadLine();
                try
                {
                    string[] dateParts = dateInput.Split('-');

                    // create new date from the parts
                    DateTime FromDate = new
                        DateTime(Convert.ToInt32(dateParts[2]),
                        Convert.ToInt32(dateParts[1]),
                        Convert.ToInt32(dateParts[0]));
                        
                    DateTime ToDate = new
                        DateTime(Convert.ToInt32(dateParts[2]),
                        Convert.ToInt32(dateParts[1]),
                        Convert.ToInt32(dateParts[0]),
                        23,59,59);

                    if (StaffManager.Slots == null)
                    {
                        Console.WriteLine("No such item.");
                        Console.WriteLine();
                        return;
                    }

                    DisplaySlots(StaffManager.Slots, dateInput, FromDate, ToDate);

                    repeat = false;

                }
                catch
                {
                    Console.WriteLine("Invalid input!");

                }
            }

        }

        private void DisplaySlots(IEnumerable<Slot> slots, string DateInput, DateTime from, DateTime to)
        {
            bool noslot = true;
            //const string format = "{0,-5}{1,-25}{2}";
            Console.WriteLine($"Slots on {DateInput}");
            Console.WriteLine("Room ID \tStartTime \t\tStaffID \tBookedInStudentID");
            foreach (var x in slots)
            {
                if (from <= x.StartTime && x.StartTime <= to)
                {
                    Console.WriteLine($"{x.RoomID} \t\t{x.StartTime} \t{x.StaffID} \t\t{x.BookedInStudentID}");
                    noslot = false;
                }

            }
            if (noslot == true)
            {
                Console.WriteLine("<No slots>");
            }

            Console.WriteLine();
        }
    }
}
