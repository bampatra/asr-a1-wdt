using System;
using Microsoft.Extensions.Configuration;

namespace AppointmentSchedulingReservation
{
    public static class Program
    {

        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("dbsetting.json").Build();

        public static string ConnectionString { get; } = Configuration["ConnectionString"];

        private static void Main(string[] args)
        {
            Staff staff = new Staff();
            Student student = new Student();
            bool repeat = true;

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Welcome to Appointment Scheduling and Reservation System");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("");

            while (repeat == true)
            {
                MenuOption();
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ListRooms();
                        break;
                    case "2":
                        ListSlots();
                        break;
                    case "3":
                        staff.StaffMenu();
                        break;
                    case "4":
                        student.StudentMenu();
                        break;
                    case "5":
                        Console.WriteLine("Terminating ASR");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        break;
                }
            }

        }

        private static void MenuOption()
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Main menu");
            Console.WriteLine("\t 1. List rooms");
            Console.WriteLine("\t 2. List slots");
            Console.WriteLine("\t 3. Staff menu");
            Console.WriteLine("\t 4. Student menu");
            Console.WriteLine("\t 5. Exit");
            Console.Write("Enter option: ");

        }

        // these methods can be implemented from interface
        private static void ListRooms()
        {

        }

        private static void ListSlots()
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
