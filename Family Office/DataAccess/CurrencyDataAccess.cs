using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class CurrencyDataAccess
    {
        private static string ConnectionString = "Data Source=example11.db;Version=3;";

        public static List<Currency> GetCurrencies()
        {
            List<Currency> currencies = new List<Currency>();
            string query = "SELECT * FROM Currency";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currencies.Add(new Currency
                        {
                            CurrencyID = reader.GetInt32(0),
                            CurrencyName = reader.GetString(1),
                            CurrencySymbol = reader.IsDBNull(2) ? null : reader.GetString(2),
                        });
                    }
                }
            }
            return currencies;
        }

        public static void AddCurrency(Currency currency)
        {
            string query = @"
                INSERT INTO Currency (CurrencyName, CurrencySymbol)
                VALUES (@CurrencyName, @CurrencySymbol)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrencyName", currency.CurrencyName);
                    command.Parameters.AddWithValue("@CurrencySymbol", currency.CurrencySymbol);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateCurrency(Currency currency)
        {
            string query = @"
                UPDATE Currency
                SET CurrencyName = @CurrencyName,
                    CurrencySymbol = @CurrencySymbol,
                WHERE CurrencyID = @CurrencyID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrencyID", currency.CurrencyID);
                    command.Parameters.AddWithValue("@CurrencyName", currency.CurrencyName);
                    command.Parameters.AddWithValue("@CurrencySymbol", (object)currency.CurrencySymbol ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteCurrency(int currencyID)
        {
            string query = "DELETE FROM Currency WHERE CurrencyID = @CurrencyID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrencyID", currencyID);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
