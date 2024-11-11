using Family_Office.Models;
using System.Data;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class ThirdPartyDataAccess
    {
        private static string ConnectionString = "Data Source=example5.db;Version=3;";

        public static List<ThirdParty> GetThirdParties()
        {
            List<ThirdParty> thirdParties = new List<ThirdParty>();
            string query = "SELECT * FROM ThirdParty";

            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    System.Diagnostics.Debug.WriteLine("Opening database connection for retrieval...");
                    connection.Open();

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var thirdParty = new ThirdParty
                                {
                                    ThirdPartyID = Convert.ToInt32(reader["ThirdPartyID"]),
                                    PartyType = reader["PartyType"] as string,
                                    PartyID = reader["PartyID"] as string,
                                    FullName = reader["FullName"] as string,
                                    FirstAssociationDate = reader["FirstAssociationDate"] as string,
                                    Photo = reader["Photo"] as byte[],
                                    MainNumberCode = reader["MainNumberCode"] as string,
                                    MainMobileNumber = reader["MainMobileNumber"] as string,
                                    AltNumberCode = reader["AltNumberCode"] as string,
                                    AltMobileNumber = reader["AltMobileNumber"] as string,
                                    EmailAddress = reader["EmailAddress"] as string,
                                    HomeAddress = reader["HomeAddress"] as string,
                                    Notes = reader["Notes"] as string,
                                    Document = reader["Document"] as byte[],
                                    CurrentStatus = reader["CurrentStatus"] as string,
                                    AadharCardNo = reader["AadharCardNo"] != DBNull.Value ? Convert.ToInt32(reader["AadharCardNo"]) : 0,
                                    PanCardNo = reader["PanCardNo"] != DBNull.Value ? Convert.ToInt32(reader["PanCardNo"]) : 0,
                                    RelationToUs = reader["RelationToUs"] as string,
                                    OpeningBalance = reader["OpeningBalance"] != DBNull.Value ? Convert.ToDouble(reader["OpeningBalance"]) : 0.0,
                                    CurrencyType = reader["CurrencyType"] as string
                                };

                                thirdParties.Add(thirdParty);
                                System.Diagnostics.Debug.WriteLine($"Successfully loaded third party: {thirdParty.FullName}");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error parsing third party row: {ex.Message}");
                                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                                // Continue to next record instead of breaking the whole operation
                                continue;
                            }
                        }
                    }
                    System.Diagnostics.Debug.WriteLine($"Total third parties loaded: {thirdParties.Count}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Critical Error in GetThirdParties: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
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

            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    System.Diagnostics.Debug.WriteLine("Opening database connection...");
                    connection.Open();

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine($"Adding Third Party: {thirdParty.FullName}");
                            System.Diagnostics.Debug.WriteLine($"Party Type: {thirdParty.PartyType}");
                            System.Diagnostics.Debug.WriteLine($"Party ID: {thirdParty.PartyID}");

                            // Handle null values and type conversions properly
                            command.Parameters.Add(new SQLiteParameter("@PartyType", DbType.String) { Value = thirdParty.PartyType ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@PartyID", DbType.String) { Value = thirdParty.PartyID ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@FullName", DbType.String) { Value = thirdParty.FullName ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@FirstAssociationDate", DbType.String) { Value = thirdParty.FirstAssociationDate ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@Photo", DbType.Binary) { Value = thirdParty.Photo ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@MainNumberCode", DbType.String) { Value = thirdParty.MainNumberCode ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@MainMobileNumber", DbType.String) { Value = thirdParty.MainMobileNumber ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@AltNumberCode", DbType.String) { Value = thirdParty.AltNumberCode ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@AltMobileNumber", DbType.String) { Value = thirdParty.AltMobileNumber ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@EmailAddress", DbType.String) { Value = thirdParty.EmailAddress ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@HomeAddress", DbType.String) { Value = thirdParty.HomeAddress ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@Notes", DbType.String) { Value = thirdParty.Notes ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@Document", DbType.Binary) { Value = thirdParty.Document ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@CurrentStatus", DbType.String) { Value = thirdParty.CurrentStatus ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@AadharCardNo", DbType.Int32) { Value = thirdParty.AadharCardNo });
                            command.Parameters.Add(new SQLiteParameter("@PanCardNo", DbType.Int32) { Value = thirdParty.PanCardNo });
                            command.Parameters.Add(new SQLiteParameter("@RelationToUs", DbType.String) { Value = thirdParty.RelationToUs ?? (object)DBNull.Value });
                            command.Parameters.Add(new SQLiteParameter("@OpeningBalance", DbType.Double) { Value = thirdParty.OpeningBalance });
                            command.Parameters.Add(new SQLiteParameter("@CurrencyType", DbType.String) { Value = thirdParty.CurrencyType ?? (object)DBNull.Value });

                            System.Diagnostics.Debug.WriteLine("Executing INSERT query...");
                            command.ExecuteNonQuery();
                            System.Diagnostics.Debug.WriteLine("Third party added successfully");
                        }
                        catch (SQLiteException sqlEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"SQLite Error: {sqlEx.Message}");
                            System.Diagnostics.Debug.WriteLine($"Error Code: {sqlEx.ErrorCode}");
                            System.Diagnostics.Debug.WriteLine($"Stack Trace: {sqlEx.StackTrace}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Critical Error in AddThirdParty: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
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