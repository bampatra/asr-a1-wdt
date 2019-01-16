using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StaffMenu : UserAbstract, IStaff
    {
        private static StaffMenu instance = null;

        // Get the instance of the object
        public static StaffMenu Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StaffMenu();
                }
                return instance;
            }

        }

        public void MenuOption()
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Staff menu");
            Console.WriteLine("\t 1. List staff");
            Console.WriteLine("\t 2. Room availability");
            Console.WriteLine("\t 3. Create slot");
            Console.WriteLine("\t 4. Remove slot");
            Console.WriteLine("\t 5. Exit");
            Console.Write("Enter option: ");
        }

        // Get the list of staff
        public void ListStaff()
        {
            Console.WriteLine("--- List staff ---");
            if (!StaffManager.Instance.Staffs.Any())
            {
                Console.WriteLine("No items present.");
                Console.WriteLine();
                return;
            }

            DisplayStaff(StaffManager.Instance.Staffs);

        }

        // Print all staffs to the console
        private void DisplayStaff(IEnumerable<User> staffs)
        {
            Console.WriteLine("ID \t\tName \t\tEmail");
            foreach (var x in staffs)
            {
                Console.WriteLine($"{x.UserID} \t\t{x.Name} \t\t{x.Email}");
            }
            Console.WriteLine();
        }

        // Get the list of available rooms on the specified date
        public void RoomAvailability()
        {

            Console.Write("Enter date for room availability (dd-mm-yyyy): ");
            string dateInput = Console.ReadLine();

            // Validate user input
            if (base.DateValidation(dateInput) == true)
            {
                // If all inputs are valid, show availabilites
                Console.WriteLine();
                Console.WriteLine($"Rooms available on {dateInput}");
                Console.WriteLine("Room name ");
                StaffManager.Instance.DisplayAvailableRoom(dateInput);

            }
            else
            {
                Console.WriteLine("Invalid input, please re-type data");
            }
            
        }

        // Create a new slot
        public void CreateSlot()
        {
           
            Console.WriteLine("--- Create slot ---");
            Console.Write("Enter room name: ");
            string roomInput = Console.ReadLine();
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            string dateInput = Console.ReadLine();
            Console.Write("Enter time for slot (hh:mm): ");
            string timeInput = Console.ReadLine();
            Console.Write("Enter staff ID: ");
            string staffInput = Console.ReadLine();

            // Validate user input
            if (base.DateValidation(dateInput) == true && 
                base.TimeValidation(timeInput) == true &&
                base.StaffValidation(staffInput) == true)
            {
                // If all inputs are valid, add to database and local memory
                StaffManager.Instance.CreateSlot(roomInput, dateInput, timeInput, staffInput);
            }
            else
            {
                Console.WriteLine("Invalid input, please re-type data");
            }

        }

        // Remove an existing slot
        public void RemoveSlot()
        {
  
            Console.WriteLine("--- Remove slot ---");
            Console.Write("Enter room name: ");
            string roomInput = Console.ReadLine();
            Console.Write("Enter date for slot (dd-mm-yyyy): ");
            string dateInput = Console.ReadLine();
            Console.Write("Enter time for slot (hh:mm): ");
            string timeInput = Console.ReadLine();

            // Validate user input
            if (base.DateValidation(dateInput) == true &&
                base.TimeValidation(timeInput) == true)
            {
                // Checks if slot exists, remove from database and local memory
                if (StaffManager.Instance.RemoveSlot(roomInput, dateInput, timeInput) == true)
                {
                    Console.WriteLine("Slot removed successfully");
                }

            }
            else
            {
                Console.WriteLine("Invalid input, please re-type data");
            }

        }


        public void ShowStaffMenu()
        {
            bool repeat = true;
            while (repeat == true)
            {
                MenuOption();
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ListStaff();
                        break;
                    case "2":
                        RoomAvailability();
                        break;
                    case "3":
                        CreateSlot();
                        break;
                    case "4":
                        RemoveSlot();
                        break;
                    case "5":
                        repeat = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        break;
                }
            }
        }
    }
}