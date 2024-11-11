using Family_Office.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.DataAccess
{
    public class FamilyOfficeDataAccess
    {
        private static string ConnectionString = "Data Source=example5.db;Version=3;";

        public static List<FamilyOffice> GetFamilyOffices()
        {
            var familyOffices = new List<FamilyOffice>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM FamilyOffice", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        familyOffices.Add(new FamilyOffice
                        {
                            FamilyOfficeID = Convert.ToInt32(reader["FamilyOfficeID"]),
                            FamilyOfficeName = reader["FamilyOfficeName"].ToString(),
                            FamilyOfficeLogo = reader["FamilyOfficeLogo"] as byte[],
                            EstablishmentDate = reader["EstablishmentDate"].ToString(),
                            Vision = reader["Vision"].ToString(),
                            HeadOfFamily = reader["HeadOfFamily"].ToString(),
                            ChiefInvestOfficer = reader["ChiefInvestOfficer"].ToString(),
                            Motto = reader["Motto"].ToString(),
                            Purpose = reader["Purpose"].ToString()
                        });
                    }
                }
            }

            return familyOffices;
        }

        public static void UpdateFamilyOffice(FamilyOffice familyOffice)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection)
                {
                    CommandText = @"
                UPDATE FamilyOffice SET 
                    FamilyOfficeName = @FamilyOfficeName,
                    FamilyOfficeLogo = @FamilyOfficeLogo,
                    EstablishmentDate = @EstablishmentDate,
                    Vision = @Vision,
                    HeadOfFamily = @HeadOfFamily,
                    ChiefInvestOfficer = @ChiefInvestOfficer,
                    Motto = @Motto,
                    Purpose = @Purpose
                WHERE FamilyOfficeID = @FamilyOfficeID;"
                };

                command.Parameters.AddWithValue("@FamilyOfficeID", familyOffice.FamilyOfficeID);
                command.Parameters.AddWithValue("@FamilyOfficeName", familyOffice.FamilyOfficeName);
                command.Parameters.AddWithValue("@FamilyOfficeLogo", familyOffice.FamilyOfficeLogo);
                command.Parameters.AddWithValue("@EstablishmentDate", familyOffice.EstablishmentDate);
                command.Parameters.AddWithValue("@Vision", familyOffice.Vision);
                command.Parameters.AddWithValue("@HeadOfFamily", familyOffice.HeadOfFamily);
                command.Parameters.AddWithValue("@ChiefInvestOfficer", familyOffice.ChiefInvestOfficer);
                command.Parameters.AddWithValue("@Motto", familyOffice.Motto);
                command.Parameters.AddWithValue("@Purpose", familyOffice.Purpose);

                command.ExecuteNonQuery();
            }
        }

        public static void AddFamilyOffice(FamilyOffice familyOffice)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection)
                {
                    CommandText = @"
                INSERT INTO FamilyOffice (
                    FamilyOfficeName, FamilyOfficeLogo, EstablishmentDate, Vision, 
                    HeadOfFamily, ChiefInvestOfficer, Motto, Purpose
                ) VALUES (
                    @FamilyOfficeName, @FamilyOfficeLogo, @EstablishmentDate, @Vision, 
                    @HeadOfFamily, @ChiefInvestOfficer, @Motto, @Purpose
                );"
                };

                command.Parameters.AddWithValue("@FamilyOfficeName", familyOffice.FamilyOfficeName);
                command.Parameters.AddWithValue("@FamilyOfficeLogo", familyOffice.FamilyOfficeLogo);
                command.Parameters.AddWithValue("@EstablishmentDate", familyOffice.EstablishmentDate);
                command.Parameters.AddWithValue("@Vision", familyOffice.Vision);
                command.Parameters.AddWithValue("@HeadOfFamily", familyOffice.HeadOfFamily);
                command.Parameters.AddWithValue("@ChiefInvestOfficer", familyOffice.ChiefInvestOfficer);
                command.Parameters.AddWithValue("@Motto", familyOffice.Motto);
                command.Parameters.AddWithValue("@Purpose", familyOffice.Purpose);

                command.ExecuteNonQuery();
            }
        }

    }
}
