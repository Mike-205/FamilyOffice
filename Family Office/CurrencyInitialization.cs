using Family_Office.DataAccess;
using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office
{
    public static class CurrencyInitialization
    {
        private static string ConnectionString = "Data Source=example11.db;Version=3;";

        public static void InitializeBaseCurrencies()
        {
            // First, check if currencies already exist to avoid duplicates
            if (!AreCurrenciesInitialized())
            {
                var baseCurrencies = new List<Currency>
                {
                    new Currency
                    {
                        CurrencyName = "USD",
                        CurrencySymbol = "$",
                    },
                    new Currency
                    {
                        CurrencyName = "EUR",
                        CurrencySymbol = "€",
                    },
                    new Currency
                    {
                        CurrencyName = "INR",
                        CurrencySymbol = "₹",
                    }
                };

                foreach (var currency in baseCurrencies)
                {
                    CurrencyDataAccess.AddCurrency(currency);
                }
            }
        }

        private static bool AreCurrenciesInitialized()
        {
            string query = "SELECT COUNT(*) FROM Currency";
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        // Optional: Method to clear all currencies if needed
        public static void ClearAllCurrencies()
        {
            string query = "DELETE FROM Currency";
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}