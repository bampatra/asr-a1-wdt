using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class MainMenu
    {
        private static MainMenu instance = null;

        // Get the instance of the object
        public static MainMenu Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainMenu();
                }
                return instance;
            }

        }

        // Get the list of rooms
        public void ListRooms()
        {
            Console.WriteLine("--- List rooms ---");

            // Check if there are any slots in the database
            if (!StaffManager.Instance.Rooms.Any())
            {
                Console.WriteLine("No items present.");
                Console.WriteLine();
                return;
            }

            DisplayRooms(StaffManager.Instance.Rooms);

        }

        // Print all rooms to the console
        private void DisplayRooms(IEnumerable<Room> rooms)
        {
            Console.WriteLine("Room name");
            foreach (var x in rooms)
            {
                Console.WriteLine(x.RoomID);
            }
            Console.WriteLine();
        }

        // Get the list of slots based on user input
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

                    // Create new date from user input
                    DateTime FromDate = new
                        DateTime(Convert.ToInt32(dateParts[2]),
                        Convert.ToInt32(dateParts[1]),
                        Convert.ToInt32(dateParts[0]));
                        
                    DateTime ToDate = new
                        DateTime(Convert.ToInt32(dateParts[2]),
                        Convert.ToInt32(dateParts[1]),
                        Convert.ToInt32(dateParts[0]),
                        23,59,59);

                    // Check if there are any slots in the database
                    if (UserManager.Instance.Slots == null)
                    {
                        Console.WriteLine("No such item.");
                        Console.WriteLine();
                        return;
                    }

                    DisplaySlots(UserManager.Instance.Slots, dateInput, FromDate, ToDate);

                    repeat = false;

                }
                catch
                {
                    Console.WriteLine("Invalid input!");

                }
            }

        }

        // Print slots to the console based on user input
        private void DisplaySlots(IEnumerable<Slot> slots, string DateInput, DateTime from, DateTime to)
        {
            bool noslot = true;
            Console.WriteLine($"Slots on {DateInput}");
            Console.WriteLine("Room ID \tStart Time \t End Time\tStaff ID \tBookings");
            foreach (var x in slots)
            {
                // Checks if a slot matches criteria (StartTime)
                if (from <= x.StartTime && x.StartTime <= to)
                {
                    Console.WriteLine($"{x.RoomID} \t\t{x.StartTime.ToShortTimeString()} " +
                    	              $"\t{x.StartTime.AddHours(1).ToShortTimeString()} \t{x.StaffID} \t\t{x.BookedInStudentID}");
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
