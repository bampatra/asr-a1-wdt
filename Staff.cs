﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentSchedulingReservation
{
    public class Staff : AppointmentSchedulingReservation.User, AppointmentSchedulingReservation.IStaff
    {
        private StaffManager StaffManager { get; } = new StaffManager();


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

        // these methods can be implemented from interface
        public void ListStaff()
        {

        }

        public void RoomAvailability()
        {
            bool repeat = true;

            while (repeat == true)
            {
                Console.Write("Enter date for room availability (dd-mm-yyyy): ");
                string dateInput = Console.ReadLine();

                if (base.DateValidation(dateInput) == true)
                {
                    // show availabilites
                    repeat = false;
                }
                else
                {
                    Console.WriteLine("Invalid input, please re-type data");
                }
            }
        }

        public void CreateSlot()
        {
            bool repeat = true;

            while (repeat == true)
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

                // Validations

                if (base.DateValidation(dateInput) == true && 
                    base.TimeValidation(timeInput) == true &&
                    base.StaffValidation(staffInput) == true)
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

        public void RemoveSlot()
        {
            bool repeat = true;

            while (repeat == true)
            {

                Console.WriteLine("--- Remove slot ---");
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
                    // Checks if slot exists, then remove from database
                    Console.WriteLine("Slot removed successfully");
                    repeat = false;
                }
                else
                {
                    Console.WriteLine("Invalid input, please re-type data");
                }

            }
        }


        public void StaffMenu()
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