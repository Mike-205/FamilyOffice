using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class ThirdPartyDataAccess
    {
        private static string ConnectionString = "Data Source=example.db;Version=3;";

        public static List<ThirdParty> GetThirdParties()
        {
            List<ThirdParty> thirdParties = new List<ThirdParty>();

            string query = "SELECT * FROM ThirdParty";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        thirdParties.Add(new ThirdParty
                        {
                            ThirdPartyID = reader.GetInt32(0),
                            PartyType = reader.GetString(1),
                            PartyID = reader.GetString(2),
                            FullName = reader.GetString(3),
                            FirstAssociationDate = reader.GetString(4),
                            Photo = reader["Photo"] as byte[], // Nullable field (can be null)
                            MainNumberCode = reader.GetString(5),
                            MainMobileNumber = reader.GetString(6),
                            AltNumberCode = reader.GetString(7),
                            AltMobileNumber = reader.GetString(8),
                            EmailAddress = reader.GetString(9),
                            HomeAddress = reader.GetString(10),
                            Notes = reader.GetString(11),
                            Document = reader["Document"] as byte[], // Nullable field
                            CurrentStatus = reader.GetString(12),
                            AadharCardNo = reader.GetInt32(13),
                            PanCardNo = reader.GetInt32(14),
                            RelationToUs = reader.GetString(15),
                            OpeningBalance = reader.GetDouble(16),
                            CurrencyType = reader.GetString(17)
                        });
                    }
                }
            }

            return thirdParties;
        }

        public static void AddThirdParty(ThirdParty thirdParty)
        {
            string query = @"
        INSERT INTO ThirdParty 
        (PartyType, PartyID, FullName, FirstAssociationDate, Photo, MainNumberCode, 
         MainMobileNumber, AltNumberCode, AltMobileNumber, EmailAddress, HomeAddress, 
         Notes, Document, CurrentStatus, AadharCardNo, PanCardNo, RelationToUs, 
         OpeningBalance, CurrencyType)
        VALUES 
        (@PartyType, @PartyID, @FullName, @FirstAssociationDate, @Photo, @MainNumberCode, 
         @MainMobileNumber, @AltNumberCode, @AltMobileNumber, @EmailAddress, @HomeAddress, 
         @Notes, @Document, @CurrentStatus, @AadharCardNo, @PanCardNo, @RelationToUs, 
         @OpeningBalance, @CurrencyType)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PartyType", thirdParty.PartyType);
                    command.Parameters.AddWithValue("@PartyID", thirdParty.PartyID);
                    command.Parameters.AddWithValue("@FullName", thirdParty.FullName);
                    command.Parameters.AddWithValue("@FirstAssociationDate", thirdParty.FirstAssociationDate);
                    command.Parameters.AddWithValue("@Photo", thirdParty.Photo);
                    command.Parameters.AddWithValue("@MainNumberCode", thirdParty.MainNumberCode);
                    command.Parameters.AddWithValue("@MainMobileNumber", thirdParty.MainMobileNumber);
                    command.Parameters.AddWithValue("@AltNumberCode", thirdParty.AltNumberCode);
                    command.Parameters.AddWithValue("@AltMobileNumber", thirdParty.AltMobileNumber);
                    command.Parameters.AddWithValue("@EmailAddress", thirdParty.EmailAddress);
                    command.Parameters.AddWithValue("@HomeAddress", thirdParty.HomeAddress);
                    command.Parameters.AddWithValue("@Notes", thirdParty.Notes);
                    command.Parameters.AddWithValue("@Document", thirdParty.Document);
                    command.Parameters.AddWithValue("@CurrentStatus", thirdParty.CurrentStatus);
                    command.Parameters.AddWithValue("@AadharCardNo", thirdParty.AadharCardNo);
                    command.Parameters.AddWithValue("@PanCardNo", thirdParty.PanCardNo);
                    command.Parameters.AddWithValue("@RelationToUs", thirdParty.RelationToUs);
                    command.Parameters.AddWithValue("@OpeningBalance", thirdParty.OpeningBalance);
                    command.Parameters.AddWithValue("@CurrencyType", thirdParty.CurrencyType);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateThirdParty(ThirdParty thirdParty)
        {
            string query = @"
        UPDATE ThirdParty
        SET PartyType = @PartyType,
            PartyID = @PartyID,
            FullName = @FullName,
            FirstAssociationDate = @FirstAssociationDate,
            Photo = @Photo,
            MainNumberCode = @MainNumberCode,
            MainMobileNumber = @MainMobileNumber,
            AltNumberCode = @AltNumberCode,
            AltMobileNumber = @AltMobileNumber,
            EmailAddress = @EmailAddress,
            HomeAddress = @HomeAddress,
            Notes = @Notes,
            Document = @Document,
            CurrentStatus = @CurrentStatus,
            AadharCardNo = @AadharCardNo,
            PanCardNo = @PanCardNo,
            RelationToUs = @RelationToUs,
            OpeningBalance = @OpeningBalance,
            CurrencyType = @CurrencyType
        WHERE ThirdPartyID = @ThirdPartyID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ThirdPartyID", thirdParty.ThirdPartyID);
                    command.Parameters.AddWithValue("@PartyType", thirdParty.PartyType);
                    command.Parameters.AddWithValue("@PartyID", thirdParty.PartyID);
                    command.Parameters.AddWithValue("@FullName", thirdParty.FullName);
                    command.Parameters.AddWithValue("@FirstAssociationDate", thirdParty.FirstAssociationDate);
                    command.Parameters.AddWithValue("@Photo", thirdParty.Photo);
                    command.Parameters.AddWithValue("@MainNumberCode", thirdParty.MainNumberCode);
                    command.Parameters.AddWithValue("@MainMobileNumber", thirdParty.MainMobileNumber);
                    command.Parameters.AddWithValue("@AltNumberCode", thirdParty.AltNumberCode);
                    command.Parameters.AddWithValue("@AltMobileNumber", thirdParty.AltMobileNumber);
                    command.Parameters.AddWithValue("@EmailAddress", thirdParty.EmailAddress);
                    command.Parameters.AddWithValue("@HomeAddress", thirdParty.HomeAddress);
                    command.Parameters.AddWithValue("@Notes", thirdParty.Notes);
                    command.Parameters.AddWithValue("@Document", thirdParty.Document);
                    command.Parameters.AddWithValue("@CurrentStatus", thirdParty.CurrentStatus);
                    command.Parameters.AddWithValue("@AadharCardNo", thirdParty.AadharCardNo);
                    command.Parameters.AddWithValue("@PanCardNo", thirdParty.PanCardNo);
                    command.Parameters.AddWithValue("@RelationToUs", thirdParty.RelationToUs);
                    command.Parameters.AddWithValue("@OpeningBalance", thirdParty.OpeningBalance);
                    command.Parameters.AddWithValue("@CurrencyType", thirdParty.CurrencyType);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteThirdParty(int thirdPartyID)
        {
            string query = "DELETE FROM ThirdParty WHERE ThirdPartyID = @ThirdPartyID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ThirdPartyID", thirdPartyID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
