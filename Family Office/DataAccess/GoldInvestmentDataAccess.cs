using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class GoldInvestmentDataAccess
    {
        private static string ConnectionString = "Data Source=example11.db;Version=3;";

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
                            GoldInvestmentID = reader.GetInt32(reader.GetOrdinal("GoldInvestmentID")),
                            GoldType = reader.GetString(reader.GetOrdinal("GoldType")),
                            InCareOf = reader.GetString(reader.GetOrdinal("InCareOf")),
                            PurchaseDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("PurchaseDate"))),
                            TotalGrams = Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("TotalGrams"))),
                            Carat = reader.GetInt32(reader.GetOrdinal("Carat")),
                            TotalPerGram = Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("TotalPerGram"))),
                            Currency = reader.GetString(reader.GetOrdinal("Currency")),
                            PurchaseVia = reader.IsDBNull(reader.GetOrdinal("PurchaseVia")) ? null : reader.GetString(reader.GetOrdinal("PurchaseVia")),
                            StorageLocation = reader.GetString(reader.GetOrdinal("StorageLocation")),
                            Country = reader.GetString(reader.GetOrdinal("Country")),
                            AnnualStorageCost = Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("AnnualStorageCost"))),
                            Document = reader["Document"] as byte[],
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
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
                (GoldType, InCareOf, PurchaseDate, TotalGrams, Carat, TotalPerGram, 
                Currency, PurchaseVia, StorageLocation, Country, AnnualStorageCost, 
                Document, Notes)
                VALUES 
                (@GoldType, @InCareOf, @PurchaseDate, @TotalGrams, @Carat, @TotalPerGram, 
                @Currency, @PurchaseVia, @StorageLocation, @Country, @AnnualStorageCost, 
                @Document, @Notes)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GoldType", investment.GoldType);
                    command.Parameters.AddWithValue("@InCareOf", investment.InCareOf);
                    command.Parameters.AddWithValue("@PurchaseDate", investment.PurchaseDate.ToString("dd-MM-yyyy"));
                    command.Parameters.AddWithValue("@TotalGrams", investment.TotalGrams);
                    command.Parameters.AddWithValue("@Carat", investment.Carat);
                    command.Parameters.AddWithValue("@TotalPerGram", investment.TotalPerGram);
                    command.Parameters.AddWithValue("@Currency", investment.Currency);
                    command.Parameters.AddWithValue("@PurchaseVia", (object)investment.PurchaseVia ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StorageLocation", investment.StorageLocation);
                    command.Parameters.AddWithValue("@Country", investment.Country);
                    command.Parameters.AddWithValue("@AnnualStorageCost", investment.AnnualStorageCost);
                    command.Parameters.AddWithValue("@Document", (object)investment.Document ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", (object)investment.Notes ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateGoldInvestment(GoldInvestment investment)
        {
            string query = @"
                UPDATE GoldInvestment
                SET GoldType = @GoldType,
                    InCareOf = @InCareOf,
                    PurchaseDate = @PurchaseDate,
                    TotalGrams = @TotalGrams,
                    Carat = @Carat,
                    TotalPerGram = @TotalPerGram,
                    Currency = @Currency,
                    PurchaseVia = @PurchaseVia,
                    StorageLocation = @StorageLocation,
                    Country = @Country,
                    AnnualStorageCost = @AnnualStorageCost,
                    Document = @Document,
                    Notes = @Notes
                WHERE GoldInvestmentID = @GoldInvestmentID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GoldInvestmentID", investment.GoldInvestmentID);
                    command.Parameters.AddWithValue("@GoldType", investment.GoldType);
                    command.Parameters.AddWithValue("@InCareOf", investment.InCareOf);
                    command.Parameters.AddWithValue("@PurchaseDate", investment.PurchaseDate.ToString("dd-MM-yyyy"));
                    command.Parameters.AddWithValue("@TotalGrams", investment.TotalGrams);
                    command.Parameters.AddWithValue("@Carat", investment.Carat);
                    command.Parameters.AddWithValue("@TotalPerGram", investment.TotalPerGram);
                    command.Parameters.AddWithValue("@Currency", investment.Currency);
                    command.Parameters.AddWithValue("@PurchaseVia", (object)investment.PurchaseVia ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StorageLocation", investment.StorageLocation);
                    command.Parameters.AddWithValue("@Country", investment.Country);
                    command.Parameters.AddWithValue("@AnnualStorageCost", investment.AnnualStorageCost);
                    command.Parameters.AddWithValue("@Document", (object)investment.Document ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", (object)investment.Notes ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

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