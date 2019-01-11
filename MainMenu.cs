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
                    DateTime testDate = new
                        DateTime(Convert.ToInt32(dateParts[2]),
                        Convert.ToInt32(dateParts[1]),
                        Convert.ToInt32(dateParts[0]));

                    Console.WriteLine(testDate);
                    repeat = false;

                }
                catch
                {
                    Console.WriteLine("Invalid input!");

                }
            }

        }

    }
}
