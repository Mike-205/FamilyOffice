using Family_Office.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.DataAccess
{
    public class PropertyInvestmentDataAccess
    {
        private static string ConnectionString = "Data Source=family_office.db;Version=3;";

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
                        investments.Add(new PropertyInvestment
                        {
                            PropertyInvestmentID = reader.GetInt32(0),
                            PropertyType = reader.GetString(1),
                            Purpose = reader.GetString(2),
                            Country = reader.GetString(3),
                            Address = reader.GetString(4),
                            Coordinates = reader.GetString(5),
                            TotalArea = reader.GetInt32(6),
                            TotalPurchasePrice = reader.GetDouble(7),
                            PricePerUnit = reader.GetDouble(8),
                            AnnualMaintenanceCost = reader.GetDouble(9),
                            BrokerCost = reader.GetDouble(10),
                            Ownership = reader.GetString(11),
                            Document = reader["Document"] as byte[],
                        });
                    }
                }
            }

            return investments;
        }

        public static void InsertPropertyInvestment(PropertyInvestment investment)
        {
            string query = @"
        INSERT INTO PropertyInvestment 
        (PropertyType, Purpose, Country, Address, Coordinates, TotalArea, 
         TotalPurchasePrice, PricePerUnit, AnnualMaintenanceCost, BrokerCost, Ownership, Document)
        VALUES 
        (@PropertyType, @Purpose, @Country, @Address, @Coordinates, @TotalArea, 
         @TotalPurchasePrice, @PricePerUnit, @AnnualMaintenanceCost, @BrokerCost, @Ownership, @Document)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PropertyType", investment.PropertyType);
                    command.Parameters.AddWithValue("@Purpose", investment.Purpose);
                    command.Parameters.AddWithValue("@Country", investment.Country);
                    command.Parameters.AddWithValue("@Address", investment.Address);
                    command.Parameters.AddWithValue("@Coordinates", investment.Coordinates);
                    command.Parameters.AddWithValue("@TotalArea", investment.TotalArea);
                    command.Parameters.AddWithValue("@TotalPurchasePrice", investment.TotalPurchasePrice);
                    command.Parameters.AddWithValue("@PricePerUnit", investment.PricePerUnit);
                    command.Parameters.AddWithValue("@AnnualMaintenanceCost", investment.AnnualMaintenanceCost);
                    command.Parameters.AddWithValue("@BrokerCost", investment.BrokerCost);
                    command.Parameters.AddWithValue("@Ownership", investment.Ownership);
                    command.Parameters.AddWithValue("@Document", investment.Document);

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
            Document = @Document
        WHERE PropertyInvestmentID = @PropertyInvestmentID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PropertyInvestmentID", investment.PropertyInvestmentID);
                    command.Parameters.AddWithValue("@PropertyType", investment.PropertyType);
                    command.Parameters.AddWithValue("@Purpose", investment.Purpose);
                    command.Parameters.AddWithValue("@Country", investment.Country);
                    command.Parameters.AddWithValue("@Address", investment.Address);
                    command.Parameters.AddWithValue("@Coordinates", investment.Coordinates);
                    command.Parameters.AddWithValue("@TotalArea", investment.TotalArea);
                    command.Parameters.AddWithValue("@TotalPurchasePrice", investment.TotalPurchasePrice);
                    command.Parameters.AddWithValue("@PricePerUnit", investment.PricePerUnit);
                    command.Parameters.AddWithValue("@AnnualMaintenanceCost", investment.AnnualMaintenanceCost);
                    command.Parameters.AddWithValue("@BrokerCost", investment.BrokerCost);
                    command.Parameters.AddWithValue("@Ownership", investment.Ownership);
                    command.Parameters.AddWithValue("@Document", investment.Document);

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
