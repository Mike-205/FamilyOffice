using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class CurrencyExchangeRateDataAccess
    {
        private static string ConnectionString = "Data Source=example11.db;Version=3;";

        public static List<CurrencyExchangeRate> GetExchangeRates()
        {
            List<CurrencyExchangeRate> rates = new List<CurrencyExchangeRate>();
            string query = "SELECT * FROM CurrencyTypes";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rates.Add(new CurrencyExchangeRate
                        {
                            CurrencyExchangeRateID = reader.GetInt32(0),
                            FromCurrencyName = reader.GetString(1),
                            ToCurrencyName = reader.GetString(2),
                            ExchangeRate = reader.GetDecimal(3)
                        });
                    }
                }
            }
            return rates;
        }

        public static void AddExchangeRate(CurrencyExchangeRate rate)
        {
            string query = @"
                INSERT INTO CurrencyTypes (FromCurrencyName, ToCurrencyName, ExchangeRate)
                VALUES (@FromCurrencyName, @ToCurrencyName, @ExchangeRate)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FromCurrencyName", rate.FromCurrencyName);
                    command.Parameters.AddWithValue("@ToCurrencyName", rate.ToCurrencyName);
                    command.Parameters.AddWithValue("@ExchangeRate", rate.ExchangeRate);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateExchangeRate(CurrencyExchangeRate rate)
        {
            string query = @"
                UPDATE CurrencyTypes
                SET FromCurrencyName = @FromCurrencyName,
                    ToCurrencyName = @ToCurrencyName,
                    ExchangeRate = @ExchangeRate
                WHERE CurrencyExchangeRateID = @CurrencyExchangeRateID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrencyExchangeRateID", rate.CurrencyExchangeRateID);
                    command.Parameters.AddWithValue("@FromCurrencyName", rate.FromCurrencyName);
                    command.Parameters.AddWithValue("@ToCurrencyName", rate.ToCurrencyName);
                    command.Parameters.AddWithValue("@ExchangeRate", rate.ExchangeRate);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteExchangeRate(int rateID)
        {
            string query = "DELETE FROM CurrencyTypes WHERE CurrencyExchangeRateID = @CurrencyExchangeRateID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrencyExchangeRateID", rateID);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Additional useful methods
        public static CurrencyExchangeRate GetExchangeRate(string fromCurrency, string toCurrency)
        {
            string query = @"
                SELECT * FROM CurrencyTypes 
                WHERE FromCurrencyName = @FromCurrencyName 
                AND ToCurrencyName = @ToCurrencyName";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FromCurrencyName", fromCurrency);
                    command.Parameters.AddWithValue("@ToCurrencyName", toCurrency);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CurrencyExchangeRate
                            {
                                CurrencyExchangeRateID = reader.GetInt32(0),
                                FromCurrencyName = reader.GetString(1),
                                ToCurrencyName = reader.GetString(2),
                                ExchangeRate = reader.GetDecimal(3)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
