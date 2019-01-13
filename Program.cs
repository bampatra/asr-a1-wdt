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
            var staff = new StaffMenu();
            var student = new StudentMenu();
            var mainmenu = new MainMenu();
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
                        mainmenu.ListRooms();
                        break;
                    case "2":
                        mainmenu.ListSlots();
                        break;
                    case "3":
                        staff.ShowStaffMenu();
                        break;
                    case "4":
                        student.ShowStudentMenu();
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



    }
}
