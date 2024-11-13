using Family_Office.Models;
using System.Data.SQLite;
using System.Diagnostics;

namespace Family_Office.DataAccess
{
    public class PropertyInvestmentDataAccess
    {
        private static string ConnectionString = "Data Source=example11.db;Version=3;";

        public static List<PropertyInvestment> GetPropertyInvestments()
        {
            List<PropertyInvestment> investments = new List<PropertyInvestment>();

            string query = "SELECT * FROM PropertyInvestment";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            var investment = new PropertyInvestment
                            {
                                PropertyInvestmentID = Convert.ToInt32(reader["PropertyInvestmentID"]),
                                PropertyType = reader["PropertyType"]?.ToString(),
                                Purpose = reader["Purpose"]?.ToString(),
                                Country = reader["Country"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Coordinates = reader["Coordinates"]?.ToString(),
                                TotalArea = !reader.IsDBNull(reader.GetOrdinal("TotalArea"))
                                    ? Convert.ToInt32(reader["TotalArea"])
                                    : 0,
                                TotalPurchasePrice = !reader.IsDBNull(reader.GetOrdinal("TotalPurchasePrice"))
                                    ? Convert.ToDouble(reader["TotalPurchasePrice"])
                                    : 0.0,
                                PricePerUnit = !reader.IsDBNull(reader.GetOrdinal("PricePerUnit"))
                                    ? Convert.ToDouble(reader["PricePerUnit"])
                                    : 0.0,
                                AnnualMaintenanceCost = !reader.IsDBNull(reader.GetOrdinal("AnnualMaintenanceCost"))
                                    ? Convert.ToDouble(reader["AnnualMaintenanceCost"])
                                    : 0.0,
                                BrokerCost = !reader.IsDBNull(reader.GetOrdinal("BrokerCost"))
                                    ? Convert.ToDouble(reader["BrokerCost"])
                                    : 0.0,
                                Ownership = reader["Ownership"]?.ToString(),
                                Document = reader["Document"] as byte[],
                                UnitOfMeasurement = reader["UnitOfMeasurement"]?.ToString(),
                                PurchaseDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("PurchaseDate"))),
                            };
                            investments.Add(investment);
                        }
                        catch (Exception ex)
                        {
                            // Log the error but continue with next record
                            Debug.WriteLine($"Error reading property investment record: {ex.Message}");
                            continue;
                        }
                    }
                }
            }

            return investments;
        }

        public static void AddPropertyInvestment(PropertyInvestment investment)
        {
            string query = @"
        INSERT INTO PropertyInvestment 
        (PropertyType, Purpose, Country, Address, Coordinates, TotalArea, 
         TotalPurchasePrice, PricePerUnit, AnnualMaintenanceCost, BrokerCost, 
         Ownership, Document, UnitOfMeasurement, PurchaseDate)
        VALUES 
        (@PropertyType, @Purpose, @Country, @Address, @Coordinates, @TotalArea, 
         @TotalPurchasePrice, @PricePerUnit, @AnnualMaintenanceCost, @BrokerCost, 
         @Ownership, @Document, @UnitOfMeasurement, @PurchaseDate)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PropertyType", (object)investment.PropertyType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Purpose", (object)investment.Purpose ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Country", (object)investment.Country ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object)investment.Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Coordinates", (object)investment.Coordinates ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TotalArea", investment.TotalArea);
                    command.Parameters.AddWithValue("@TotalPurchasePrice", investment.TotalPurchasePrice);
                    command.Parameters.AddWithValue("@PricePerUnit", investment.PricePerUnit);
                    command.Parameters.AddWithValue("@AnnualMaintenanceCost", investment.AnnualMaintenanceCost);
                    command.Parameters.AddWithValue("@BrokerCost", investment.BrokerCost);
                    command.Parameters.AddWithValue("@Ownership", (object)investment.Ownership ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Document", (object)investment.Document ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UnitOfMeasurement", (object)investment.UnitOfMeasurement ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PurchaseDate", investment.PurchaseDate.ToString("dd-MM-yyyy"));
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdatePropertyInvestment(PropertyInvestment investment)
        {
            string query = @"
        UPDATE PropertyInvestment
        SET PropertyType = @PropertyType,
            Purpose = @Purpose,
            Country = @Country,
            Address = @Address,
            Coordinates = @Coordinates,
            TotalArea = @TotalArea,
            TotalPurchasePrice = @TotalPurchasePrice,
            PricePerUnit = @PricePerUnit,
            AnnualMaintenanceCost = @AnnualMaintenanceCost,
            BrokerCost = @BrokerCost,
            Ownership = @Ownership,
            Document = @Document,
            UnitOfMeasurement = @UnitOfMeasurement,
            PurchaseDate = @PurchaseDate
        WHERE PropertyInvestmentID = @PropertyInvestmentID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PropertyInvestmentID", investment.PropertyInvestmentID);
                    command.Parameters.AddWithValue("@PropertyType", (object)investment.PropertyType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Purpose", (object)investment.Purpose ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Country", (object)investment.Country ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", (object)investment.Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Coordinates", (object)investment.Coordinates ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TotalArea", investment.TotalArea);
                    command.Parameters.AddWithValue("@TotalPurchasePrice", investment.TotalPurchasePrice);
                    command.Parameters.AddWithValue("@PricePerUnit", investment.PricePerUnit);
                    command.Parameters.AddWithValue("@AnnualMaintenanceCost", investment.AnnualMaintenanceCost);
                    command.Parameters.AddWithValue("@BrokerCost", investment.BrokerCost);
                    command.Parameters.AddWithValue("@Ownership", (object)investment.Ownership ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Document", (object)investment.Document ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UnitOfMeasurement", (object)investment.UnitOfMeasurement ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PurchaseDate", investment.PurchaseDate.ToString("dd-MM-yyyy"));
                    command.ExecuteNonQuery();
                }
            }
        }

        // For later when the UI updates
        public static void DeletePropertyInvestment(int propertyInvestmentID)
        {
            string query = "DELETE FROM PropertyInvestment WHERE PropertyInvestmentID = @PropertyInvestmentID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PropertyInvestmentID", propertyInvestmentID);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
