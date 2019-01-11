using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StudentMenu : AppointmentSchedulingReservation.UserAbstract, AppointmentSchedulingReservation.IStudent
    {
        private StudentManager StudentManager { get; } = new StudentManager();

        public void MenuOption()
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Main menu");
            Console.WriteLine("\t 1. List students");
            Console.WriteLine("\t 2. Staff availability");
            Console.WriteLine("\t 3. Make booking");
            Console.WriteLine("\t 4. Cancel booking");
            Console.WriteLine("\t 5. Exit");
            Console.Write("Enter option: ");
        }

        // these methods can be implemented from interface
        public void ListStudents()
        {
            Console.WriteLine("--- List students ---");
            if (!StudentManager.Students.Any())
            {
                Console.WriteLine("No items present.");
                Console.WriteLine();
                return;
            }

            DisplayStudents(StudentManager.Students);

        }

        private void DisplayStudents(IEnumerable<User> students)
        {
            //const string format = "{0,-5}{1,-25}{2}";
            Console.WriteLine("ID \tName \tEmail");
            foreach (var x in students)
            {
                Console.WriteLine($"{x.UserID} \t\t{x.Name} \t\t{x.Email}");
            }
            Console.WriteLine();
        }

        public void StaffAvailability()
        {
            bool repeat = true;
            while (repeat == true)
            {
                Console.WriteLine("--- Staff availibility ---");
                Console.Write("Enter date for staff availability (dd-mm-yyyy): ");
                string dateInput = Console.ReadLine();
                Console.Write("Enter staff ID: ");
                string staffInput = Console.ReadLine();

                // Validations
                if (base.DateValidation(dateInput) == true &&
                    base.StaffValidation(staffInput) == true)
                {
                    // Show staff
                    repeat = false;
                }
            }

        }

        public void MakeBooking()
        {
            bool repeat = true;

            while (repeat == true)
            {
                Console.WriteLine("--- Create slot ---");
                Console.Write("Enter room name: ");
                string roomInput = Console.ReadLine();
                Console.Write("Enter date for slots (dd-mm-yyyy): ");
                string dateInput = Console.ReadLine();
                Console.Write("Enter time for slots (hh:mm): ");
                string timeInput = Console.ReadLine();
                Console.Write("Enter student ID: ");
                string studentInput = Console.ReadLine();

                // Validations
                if (base.DateValidation(dateInput) == true &&
                    base.TimeValidation(timeInput) == true &&
                    base.StudentValidation(studentInput) == true)
                {
                    // If all inputs are valid, then add to database
                    Console.WriteLine("Slot created successfully");
                    repeat = false;
                }
                else
                {
                    Console.WriteLine("Invalid input, please re-type data");
                }
            }

        }

        public void CancelBooking()
        {
            bool repeat = true;

            while (repeat == true)
            {
                Console.WriteLine("--- Cancel booking ---");
                Console.Write("Enter room name: ");
                string roomInput = Console.ReadLine();
                Console.Write("Enter date for slot (dd-mm-yyyy): ");
                string dateInput = Console.ReadLine();
                Console.Write("Enter time for slot (hh:mm): ");
                string timeInput = Console.ReadLine();

                // Validations
                if (base.DateValidation(dateInput) == true &&
                    base.TimeValidation(timeInput) == true)
                {
                    // If all inputs are valid, then remove from database
                    Console.WriteLine("Slot removed successfully");
                    repeat = false;
                }
                else
                {
                    Console.WriteLine("Invalid input, please re-type data");
                }

            }

        }

        public void ShowStudentMenu()
        {
            bool repeat = true;
            while (repeat == true)
            {
                MenuOption();
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ListStudents();
                        break;
                    case "2":
                        StaffAvailability();
                        break;
                    case "3":
                        MakeBooking();
                        break;
                    case "4":
                        CancelBooking();
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
