using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class FamilyMemberDataAccess
    {
        private static string ConnectionString = "Data Source=example.db;Version=3;";

        public static List<FamilyMember> GetFamilyMembers()
        {
            var familyMembers = new List<FamilyMember>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM FamilyMember", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        familyMembers.Add(new FamilyMember
                        {
                            FamilyMemberID = Convert.ToInt32(reader["FamilyMemberID"]),
                            CustomMemberID = reader["CustomMemberID"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            NickName = reader["NickName"].ToString(),
                            ProfilePicture = reader["ProfilePicture"].ToString(),
                            DOB = reader["DOB"].ToString(),
                            FamilyRelation = reader["FamilyRelation"].ToString(),
                            Role = reader["Role"].ToString(),
                            Status = reader["Status"].ToString(),
                            JoiningFamilyDate = reader["JoiningFamilyDate"].ToString(),
                            CountryCode1 = reader["CountryCode1"].ToString(),
                            Telephone1 = reader["Telephone1"].ToString(),
                            CountryCode2 = reader["CountryCode2"].ToString(),
                            Telephone2 = reader["Telephone2"].ToString(),
                            PersonalEmail = reader["PersonalEmail"].ToString(),
                            FamilyEmail = reader["FamilyEmail"].ToString(),
                            HomeAddress = reader["HomeAddress"].ToString(),
                            Nationality = reader["Nationality"].ToString(),
                            CurrentResidence = reader["CurrentResidence"].ToString(),
                            PassPortNo = Convert.ToInt32(reader["PassPortNo"]),
                            PassportExpiryDate = reader["PassportExpiryDate"].ToString(),
                            EmiratesID = Convert.ToInt32(reader["EmiratesID"]),
                            EmiratesIDExpiryDate = reader["EmiratesIDExpiryDate"].ToString(),
                            AadharNo = Convert.ToInt32(reader["AadharNo"]),
                            PanCardNumber = Convert.ToInt32(reader["PanCardNumber"]),
                            EducationStatus = reader["EducationStatus"].ToString(),
                            MainSkills = reader["MainSkills"].ToString(),
                            SecondarySkills = reader["SecondarySkills"].ToString(),
                            EducatedFrom = reader["EducatedFrom"].ToString(),
                            Document = reader["Document"] as byte[]
                        });
                    }
                }
            }

            return familyMembers;
        }

        public static void AddFamilyMember(FamilyMember member)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection)
                {
                    CommandText = @"
                INSERT INTO FamilyMember (CustomMemberID, FullName, NickName, ProfilePicture, DOB, FamilyRelation, Role, Status, JoiningFamilyDate, 
                CountryCode1, Telephone1, CountryCode2, Telephone2, PersonalEmail, FamilyEmail, HomeAddress, Nationality, CurrentResidence, 
                PassPortNo, PassportExpiryDate, EmiratesID, EmiratesIDExpiryDate, AadharNo, PanCardNumber, EducationStatus, 
                MainSkills, SecondarySkills, EducatedFrom, Document) 
                VALUES (@CustomMemberID, @FullName, @NickName, @ProfilePicture, @DOB, @FamilyRelation, @Role, @Status, @JoiningFamilyDate, 
                @CountryCode1, @Telephone1, @CountryCode2, @Telephone2, @PersonalEmail, @FamilyEmail, @HomeAddress, @Nationality, @CurrentResidence, 
                @PassPortNo, @PassportExpiryDate, @EmiratesID, @EmiratesIDExpiryDate, @AadharNo, @PanCardNumber, @EducationStatus, 
                @MainSkills, @SecondarySkills, @EducatedFrom, @Document)"
                };

                command.Parameters.AddWithValue("@CustomMemberID", member.CustomMemberID);
                command.Parameters.AddWithValue("@FullName", member.FullName);
                command.Parameters.AddWithValue("@NickName", member.NickName);
                command.Parameters.AddWithValue("@ProfilePicture", member.ProfilePicture);
                command.Parameters.AddWithValue("@DOB", member.DOB);
                command.Parameters.AddWithValue("@FamilyRelation", member.FamilyRelation);
                command.Parameters.AddWithValue("@Role", member.Role);
                command.Parameters.AddWithValue("@Status", member.Status);
                command.Parameters.AddWithValue("@JoiningFamilyDate", member.JoiningFamilyDate);
                command.Parameters.AddWithValue("@CountryCode1", member.CountryCode1);
                command.Parameters.AddWithValue("@Telephone1", member.Telephone1);
                command.Parameters.AddWithValue("@CountryCode2", member.CountryCode2);
                command.Parameters.AddWithValue("@Telephone2", member.Telephone2);
                command.Parameters.AddWithValue("@PersonalEmail", member.PersonalEmail);
                command.Parameters.AddWithValue("@FamilyEmail", member.FamilyEmail);
                command.Parameters.AddWithValue("@HomeAddress", member.HomeAddress);
                command.Parameters.AddWithValue("@Nationality", member.Nationality);
                command.Parameters.AddWithValue("@CurrentResidence", member.CurrentResidence);
                command.Parameters.AddWithValue("@PassPortNo", member.PassPortNo);
                command.Parameters.AddWithValue("@PassportExpiryDate", member.PassportExpiryDate);
                command.Parameters.AddWithValue("@EmiratesID", member.EmiratesID);
                command.Parameters.AddWithValue("@EmiratesIDExpiryDate", member.EmiratesIDExpiryDate);
                command.Parameters.AddWithValue("@AadharNo", member.AadharNo);
                command.Parameters.AddWithValue("@PanCardNumber", member.PanCardNumber);
                command.Parameters.AddWithValue("@EducationStatus", member.EducationStatus);
                command.Parameters.AddWithValue("@MainSkills", member.MainSkills);
                command.Parameters.AddWithValue("@SecondarySkills", member.SecondarySkills);
                command.Parameters.AddWithValue("@EducatedFrom", member.EducatedFrom);
                command.Parameters.AddWithValue("@Document", member.Document);

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateFamilyMember(FamilyMember member)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection)
                {
                    CommandText = @"
                UPDATE FamilyMember SET 
                    CustomMemberID = @CustomMemberID,
                    FullName = @FullName,
                    NickName = @NickName,
                    ProfilePicture = @ProfilePicture,
                    DOB = @DOB,
                    FamilyRelation = @FamilyRelation,
                    Role = @Role,
                    Status = @Status,
                    JoiningFamilyDate = @JoiningFamilyDate,
                    CountryCode1 = @CountryCode1,
                    Telephone1 = @Telephone1,
                    CountryCode2 = @CountryCode2,
                    Telephone2 = @Telephone2,
                    PersonalEmail = @PersonalEmail,
                    FamilyEmail = @FamilyEmail,
                    HomeAddress = @HomeAddress,
                    Nationality = @Nationality,
                    CurrentResidence = @CurrentResidence,
                    PassPortNo = @PassPortNo,
                    PassportExpiryDate = @PassportExpiryDate,
                    EmiratesID = @EmiratesID,
                    EmiratesIDExpiryDate = @EmiratesIDExpiryDate,
                    AadharNo = @AadharNo,
                    PanCardNumber = @PanCardNumber,
                    EducationStatus = @EducationStatus,
                    MainSkills = @MainSkills,
                    SecondarySkills = @SecondarySkills,
                    EducatedFrom = @EducatedFrom,
                    Document = @Document
                WHERE FamilyMemberID = @FamilyMemberID"
                };

                command.Parameters.AddWithValue("@FamilyMemberID", member.FamilyMemberID);
                command.Parameters.AddWithValue("@CustomMemberID", member.CustomMemberID);
                command.Parameters.AddWithValue("@FullName", member.FullName);
                command.Parameters.AddWithValue("@NickName", member.NickName);
                command.Parameters.AddWithValue("@ProfilePicture", member.ProfilePicture);
                command.Parameters.AddWithValue("@DOB", member.DOB);
                command.Parameters.AddWithValue("@FamilyRelation", member.FamilyRelation);
                command.Parameters.AddWithValue("@Role", member.Role);
                command.Parameters.AddWithValue("@Status", member.Status);
                command.Parameters.AddWithValue("@JoiningFamilyDate", member.JoiningFamilyDate);
                command.Parameters.AddWithValue("@CountryCode1", member.CountryCode1);
                command.Parameters.AddWithValue("@Telephone1", member.Telephone1);
                command.Parameters.AddWithValue("@CountryCode2", member.CountryCode2);
                command.Parameters.AddWithValue("@Telephone2", member.Telephone2);
                command.Parameters.AddWithValue("@PersonalEmail", member.PersonalEmail);
                command.Parameters.AddWithValue("@FamilyEmail", member.FamilyEmail);
                command.Parameters.AddWithValue("@HomeAddress", member.HomeAddress);
                command.Parameters.AddWithValue("@Nationality", member.Nationality);
                command.Parameters.AddWithValue("@CurrentResidence", member.CurrentResidence);
                command.Parameters.AddWithValue("@PassPortNo", member.PassPortNo);
                command.Parameters.AddWithValue("@PassportExpiryDate", member.PassportExpiryDate);
                command.Parameters.AddWithValue("@EmiratesID", member.EmiratesID);
                command.Parameters.AddWithValue("@EmiratesIDExpiryDate", member.EmiratesIDExpiryDate);
                command.Parameters.AddWithValue("@AadharNo", member.AadharNo);
                command.Parameters.AddWithValue("@PanCardNumber", member.PanCardNumber);
                command.Parameters.AddWithValue("@EducationStatus", member.EducationStatus);
                command.Parameters.AddWithValue("@MainSkills", member.MainSkills);
                command.Parameters.AddWithValue("@SecondarySkills", member.SecondarySkills);
                command.Parameters.AddWithValue("@EducatedFrom", member.EducatedFrom);
                command.Parameters.AddWithValue("@Document", member.Document);

                command.ExecuteNonQuery();
            }
        }

        public static void DeleteFamilyMember(int familyMemberID)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection)
                {
                    CommandText = "DELETE FROM FamilyMember WHERE FamilyMemberID = @FamilyMemberID"
                };

                command.Parameters.AddWithValue("@FamilyMemberID", familyMemberID);
                command.ExecuteNonQuery();
            }
        }
    }
}
