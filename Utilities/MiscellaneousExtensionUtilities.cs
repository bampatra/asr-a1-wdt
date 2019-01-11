using System.Data;
using System.Data.SqlClient;

namespace AppointmentSchedulingReservation
{
    public static class MiscellaneousExtensionUtilities
    {
        public static bool IsWithinRange(this int value, int min, int max) => value >= min && value <= max;
        public static bool HasMoreThanNDecimalPlaces(this decimal value, int n) => decimal.Round(value, n) != value;

        public static SqlConnection CreateConnection(this string connectionString) =>
            new SqlConnection(connectionString);

        public static DataTable GetDataTable(this SqlCommand command)
        {
            var table = new DataTable();
            new SqlDataAdapter(command).Fill(table);

            return table;
        }
    }
}
