using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class CurrencyTypeDataAccess
    {
        private static string ConnectionString = "Data Source=example5.db;Version=3;";

        public static List<CurrencyType> GetCurrencyTypes()
        {
            List<CurrencyType> currencyTypes = new List<CurrencyType>();

            string query = "SELECT * FROM CurrencyTypes";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currencyTypes.Add(new CurrencyType
                        {
                            CurrencyCode = reader.GetString(0),
                            CurrencyName = reader.GetString(1),
                            CurrencySymbol = reader.GetString(2),
                            ExchangeRate = reader.GetDouble(3)
                        });
                    }
                }
            }

            return currencyTypes;
        }

        public static void AddCurrencyType(CurrencyType currencyType)
        {
            string query = @"
        INSERT INTO CurrencyTypes (CurrencyCode, CurrencyName, CurrencySymbol, ExchangeRate)
        VALUES (@CurrencyCode, @CurrencyName, @CurrencySymbol, @ExchangeRate)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrencyCode", currencyType.CurrencyCode);
                    command.Parameters.AddWithValue("@CurrencyName", currencyType.CurrencyName);
                    command.Parameters.AddWithValue("@CurrencySymbol", currencyType.CurrencySymbol);
                    command.Parameters.AddWithValue("@ExchangeRate", currencyType.ExchangeRate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateCurrencyType(CurrencyType currencyType)
        {
            string query = @"
        UPDATE CurrencyTypes
        SET CurrencyName = @CurrencyName,
            CurrencySymbol = @CurrencySymbol,
            ExchangeRate = @ExchangeRate
        WHERE CurrencyCode = @CurrencyCode";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrencyCode", currencyType.CurrencyCode);
                    command.Parameters.AddWithValue("@CurrencyName", currencyType.CurrencyName);
                    command.Parameters.AddWithValue("@CurrencySymbol", currencyType.CurrencySymbol);
                    command.Parameters.AddWithValue("@ExchangeRate", currencyType.ExchangeRate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteCurrencyType(string currencyCode)
        {
            string query = "DELETE FROM CurrencyTypes WHERE CurrencyCode = @CurrencyCode";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrencyCode", currencyCode);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
