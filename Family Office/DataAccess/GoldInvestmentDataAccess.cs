using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class GoldInvestmentDataAccess
    {
        private static string ConnectionString = "Data Source=example5.db;Version=3;";

        public static List<GoldInvestment> GetGoldInvestments()
        {
            List<GoldInvestment> investments = new List<GoldInvestment>();

            string query = "SELECT * FROM GoldInvestment";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        investments.Add(new GoldInvestment
                        {
                            GoldInvestmentID = reader.GetInt32(0),
                            GoldType = reader.GetString(1),
                            TotalGrams = reader.GetDouble(2),
                            Carat = reader.GetInt32(3),
                            TotalPerGram = reader.GetDouble(4),
                            StorageLocation = reader.GetString(5),
                            Country = reader.GetString(6),
                            AnnualStorageCost = reader.GetDouble(7),
                            AnnualMaintenanceCost = reader.GetDouble(8),
                            Document = reader["Document"] as byte[],
                            PurchaseDate = reader.GetString(9),
                            InCareOf = reader.GetString(10),
                            Currency = reader.GetString(11)
                        });
                    }
                }
            }

            return investments;
        }

        public static void AddGoldInvestment(GoldInvestment investment)
        {
            string query = @"
        INSERT INTO GoldInvestment 
        (GoldType, TotalGrams, Carat, TotalPerGram, StorageLocation, Country, 
         AnnualStorageCost, AnnualMaintenanceCost, Document, PurchaseDate, InCareOf)
        VALUES 
        (@GoldType, @TotalGrams, @Carat, @TotalPerGram, @StorageLocation, @Country, 
         @AnnualStorageCost, @AnnualMaintenanceCost, @Document, @PurchaseDate, @InCareOf)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GoldType", investment.GoldType);
                    command.Parameters.AddWithValue("@TotalGrams", investment.TotalGrams);
                    command.Parameters.AddWithValue("@Carat", investment.Carat);
                    command.Parameters.AddWithValue("@TotalPerGram", investment.TotalPerGram);
                    command.Parameters.AddWithValue("@StorageLocation", investment.StorageLocation);
                    command.Parameters.AddWithValue("@Country", investment.Country);
                    command.Parameters.AddWithValue("@AnnualStorageCost", investment.AnnualStorageCost);
                    command.Parameters.AddWithValue("@AnnualMaintenanceCost", investment.AnnualMaintenanceCost);
                    command.Parameters.AddWithValue("@Document", investment.Document);
                    command.Parameters.AddWithValue("@PurchaseDate", investment.PurchaseDate);
                    command.Parameters.AddWithValue("@InCareOf", investment.InCareOf);
                    command.Parameters.AddWithValue("@Currency", investment.Currency);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateGoldInvestment(GoldInvestment investment)
        {
            string query = @"
        UPDATE GoldInvestment
        SET GoldType = @GoldType,
            TotalGrams = @TotalGrams,
            Carat = @Carat,
            TotalPerGram = @TotalPerGram,
            StorageLocation = @StorageLocation,
            Country = @Country,
            AnnualStorageCost = @AnnualStorageCost,
            AnnualMaintenanceCost = @AnnualMaintenanceCost,
            Document = @Document,
            PurchaseDate = @PurchaseDate,
            InCareOf = @InCareOf
        WHERE GoldInvestmentID = @GoldInvestmentID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GoldInvestmentID", investment.GoldInvestmentID);
                    command.Parameters.AddWithValue("@GoldType", investment.GoldType);
                    command.Parameters.AddWithValue("@TotalGrams", investment.TotalGrams);
                    command.Parameters.AddWithValue("@Carat", investment.Carat);
                    command.Parameters.AddWithValue("@TotalPerGram", investment.TotalPerGram);
                    command.Parameters.AddWithValue("@StorageLocation", investment.StorageLocation);
                    command.Parameters.AddWithValue("@Country", investment.Country);
                    command.Parameters.AddWithValue("@AnnualStorageCost", investment.AnnualStorageCost);
                    command.Parameters.AddWithValue("@AnnualMaintenanceCost", investment.AnnualMaintenanceCost);
                    command.Parameters.AddWithValue("@Document", investment.Document);
                    command.Parameters.AddWithValue("@PurchaseDate", investment.PurchaseDate);
                    command.Parameters.AddWithValue("@InCareOf", investment.InCareOf);
                    command.Parameters.AddWithValue("@Currency", investment.Currency);
                    command.ExecuteNonQuery();
                }
            }
        }

        // For later when the UI updates
        public static void DeleteGoldInvestment(int goldInvestmentID)
        {
            string query = "DELETE FROM GoldInvestment WHERE GoldInvestmentID = @GoldInvestmentID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GoldInvestmentID", goldInvestmentID);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
