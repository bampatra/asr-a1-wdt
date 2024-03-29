﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class StudentMenu : UserAbstract, IStudent
    {
        private static StudentMenu instance = null;

        // Get the instance of the object
        public static StudentMenu Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentMenu();
                }
                return instance;
            }

        }

        public void MenuOption()
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Student menu");
            Console.WriteLine("\t 1. List students");
            Console.WriteLine("\t 2. Staff availability");
            Console.WriteLine("\t 3. Make booking");
            Console.WriteLine("\t 4. Cancel booking");
            Console.WriteLine("\t 5. Exit");
            Console.Write("Enter option: ");
        }

        // Get the list of student
        public void ListStudents()
        {
            Console.WriteLine("--- List students ---");
            if (!StudentManager.Instance.Students.Any())
            {
                Console.WriteLine("No items present.");
                Console.WriteLine();
                return;
            }

            DisplayStudents(StudentManager.Instance.Students);

        }

        // Print all students to the console
        private void DisplayStudents(IEnumerable<User> students)
        {
            //const string format = "{0,-5}{1,-25}{2}";
            Console.WriteLine("ID \t\tName \t\tEmail");
            foreach (var x in students)
            {
                Console.WriteLine($"{x.UserID} \t{x.Name} \t\t{x.Email}");
            }
            Console.WriteLine();
        }

        // Check a staff's availability on the specified date
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

                // Validate user input
                if (base.DateValidation(dateInput) == true &&
                    base.StaffValidation(staffInput) == true)
                {
                    // Show availability
                    DisplayStaffAvailability(StudentManager.Instance.Slots, dateInput, staffInput);

                    repeat = false;
                }
                else
                {
                    Console.WriteLine("Invalid data! Please re-type.");
                }
            }

        }

        // Print the specified staff's availability on the specified date to the console
        public void DisplayStaffAvailability(IEnumerable<Slot> slots, string DateInput, string staffID)
        {

            string[] dateParts = DateInput.Split('-');
            bool noslot = true;

            DateTime FromDate = new
                DateTime(Convert.ToInt32(dateParts[2]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[0]));

            DateTime ToDate = new
                DateTime(Convert.ToInt32(dateParts[2]),
                Convert.ToInt32(dateParts[1]),
                Convert.ToInt32(dateParts[0]),
                23, 59, 59);

            Console.WriteLine($"Staff {staffID} availability on {DateInput}: ");
            Console.WriteLine("Room Name \tStart Time \tEnd Time");
            foreach (var x in slots)
            {
                // If the date matches user input and the slot has not been booked
                if (FromDate <= x.StartTime && x.StartTime <= ToDate && 
                    x.StaffID == staffID && (x.BookedInStudentID is DBNull || x.BookedInStudentID is null))
                {
                    Console.WriteLine($"{x.RoomID} \t\t{x.StartTime.ToShortTimeString()} " +
                    	               $"\t{x.StartTime.AddHours(1).ToShortTimeString()}");
                    noslot = false;
                }

            }
            if (noslot == true)
            {
                Console.WriteLine("<No slots>");
            }
        }

        // Book an existing slot
        public void MakeBooking()
        {

            Console.WriteLine("--- Make booking ---");
            Console.Write("Enter room name: ");
            string roomInput = Console.ReadLine();
            Console.Write("Enter date for slots (dd-mm-yyyy): ");
            string dateInput = Console.ReadLine();
            Console.Write("Enter time for slots (hh:mm): ");
            string timeInput = Console.ReadLine();
            Console.Write("Enter student ID: ");
            string studentInput = Console.ReadLine();

            // Validate user input
            if (base.DateValidation(dateInput) == true &&
                base.TimeValidation(timeInput) == true &&
                base.StudentValidation(studentInput) == true)
            {
                // Convert date and time into one datetime object
                string[] dateParts = dateInput.Split('-');
                string[] timeParts = timeInput.Split(':');
                DateTime newDate = new
                    DateTime(Convert.ToInt32(dateParts[2]),
                    Convert.ToInt32(dateParts[1]),
                    Convert.ToInt32(dateParts[0]),
                    Convert.ToInt32(timeParts[0]),
                    Convert.ToInt32(timeParts[1]),
                    0);


                var item = StudentManager.Instance.GetSlot(roomInput, newDate);
                if (item == null)
                {
                    Console.WriteLine("Slot does not exist");
                    Console.WriteLine();
                }
                else
                {
                    // If all inputs are valid, update database and local memory
                    if (StudentManager.Instance.CheckMaxBooking(dateInput, studentInput, roomInput) == true)
                    {
                        StudentManager.Instance.MakeBooking(item, studentInput);
                    }
                    else
                    {
                        Console.WriteLine("Maximum limit has been reached. Unable to create slot."); 
                    }
                }

            }
            else
            {
                Console.WriteLine("Invalid input, please re-type data");
            }
            
        }

        // Cancel a booked slot
        public void CancelBooking()
        {
            Console.WriteLine("--- Cancel booking ---");
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
                // Convert date and time into one datetime object
                string[] dateParts = dateInput.Split('-');
                string[] timeParts = timeInput.Split(':');
                DateTime newDate = new
                    DateTime(Convert.ToInt32(dateParts[2]),
                    Convert.ToInt32(dateParts[1]),
                    Convert.ToInt32(dateParts[0]),
                    Convert.ToInt32(timeParts[0]),
                    Convert.ToInt32(timeParts[1]),
                    0);

                var item = StudentManager.Instance.GetSlot(roomInput, newDate);
                if (item == null)
                {
                    Console.WriteLine("Slot does not exist");
                    Console.WriteLine();
                }
                else
                {
                    // update slot in database and local memory
                    StudentManager.Instance.CancelBooking(item);
                }
            }
            else
            {
                Console.WriteLine("Invalid input, please re-type data");
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
