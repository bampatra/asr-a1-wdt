using System;
using System.Text.RegularExpressions;

namespace AppointmentSchedulingReservation
{
    public abstract class UserAbstract
    {
        protected bool DateValidation(string date)
        {
            try
            {
                string[] dateParts = date.Split('-');

                // create new date from the parts
                DateTime testDate = new
                    DateTime(Convert.ToInt32(dateParts[2]),
                    Convert.ToInt32(dateParts[1]),
                    Convert.ToInt32(dateParts[0]));

                //Console.WriteLine(testDate);
                return true;

            }
            catch
            {
                Console.WriteLine("Invalid date format!");
                return false;

            }
        }

        protected bool TimeValidation(string time)
        {
            try
            {
                var dateTime = DateTime.ParseExact(time, "H:mm", null, System.Globalization.DateTimeStyles.None);

                if (dateTime.Minute != 0)
                {
                    Console.WriteLine("Invalid time format");
                    return false;
                }
                else
                    if (9 <= dateTime.Hour && dateTime.Hour <= 13)
                {
                    //Console.WriteLine(dateTime);
                    return true;
                }
                else
                {
                    Console.WriteLine("Outside working hour");
                    return false;
                }
            }

            catch (System.FormatException)
            {
                return false;
            }


        }

        protected bool StudentValidation(string studentID)
        {
            // s followed by 7 numbers
            var staffRegex = @"^[s]\d{7}$";
            Match match = Regex.Match(studentID, staffRegex);
            if (match.Success)
            {
                return true;
            }
            else
            {
                Console.WriteLine("INVALID STUDENT ID");
                return false;
            }

        }

        protected bool StaffValidation(string staffID)
        {
            // e followed by 5 numbers
            var staffRegex = @"^[e]\d{5}$";
            Match match = Regex.Match(staffID, staffRegex);
            if (match.Success)
            {
                return true;
            }
            else
            {
                Console.WriteLine("INVALID STAFF ID");
                return false;
            }

        }

        }
    }
