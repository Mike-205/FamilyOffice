using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class CashAssetDataAccess
    {

        private static string ConnectionString = "Data Source=example.db;Version=3;";

        public static List<CashAsset> GetCashAssets()
        {
            List<CashAsset> cashAssets = new List<CashAsset>();

            string query = "SELECT * FROM CashAsset";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cashAssets.Add(new CashAsset
                        {
                            CashAssetID = reader.GetInt32(0),
                            Location = reader.GetString(1),
                            InCareOf = reader.GetString(2),
                            Currency = reader.GetString(3),
                            Amount = reader.GetDouble(4)
                        });
                    }
                }
            }

            return cashAssets;
        }

        public static void AddCashAsset(CashAsset cashAsset)
        {
            string query = @"
        INSERT INTO CashAsset 
        (Location, InCareOf, Currency, Amount)
        VALUES 
        (@Location, @InCareOf, @Currency, @Amount)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Location", cashAsset.Location);
                    command.Parameters.AddWithValue("@InCareOf", cashAsset.InCareOf);
                    command.Parameters.AddWithValue("@Currency", cashAsset.Currency);
                    command.Parameters.AddWithValue("@Amount", cashAsset.Amount);

                    command.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateCashAsset(CashAsset cashAsset)
        {
            string query = @"
        UPDATE CashAsset
        SET Location = @Location,
            InCareOf = @InCareOf,
            Currency = @Currency,
            Amount = @Amount
        WHERE CashAssetID = @CashAssetID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CashAssetID", cashAsset.CashAssetID);
                    command.Parameters.AddWithValue("@Location", cashAsset.Location);
                    command.Parameters.AddWithValue("@InCareOf", cashAsset.InCareOf);
                    command.Parameters.AddWithValue("@Currency", cashAsset.Currency);
                    command.Parameters.AddWithValue("@Amount", cashAsset.Amount);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
