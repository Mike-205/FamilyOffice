using Family_Office.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.DataAccess
{
    internal class BankAccountDataAccess
    {
        private static string ConnectionString = "Data Source=family_office.db;Version=3;";

        public static List<BankAccount> GetBankAccounts()
        {
            List<BankAccount> accounts = new List<BankAccount>();

            string query = "SELECT * FROM BankAccount";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accounts.Add(new BankAccount
                        {
                            AccountID = reader.GetInt32(0),
                            BankName = reader.GetString(1),
                            Country = reader.GetString(2),
                            AccountHolder = reader.GetString(3),
                            SwiftCode = reader.GetString(4),
                            AccountType = reader.GetString(5),
                            BranchLocation = reader.GetString(6),
                            AccountNumber = reader.GetInt32(7),
                            Nominee = reader.GetString(8),
                            OpeningBalance = reader.GetDouble(9),
                        });
                    }
                }
            }

            return accounts;
        }

        public static void AddBankAccount(BankAccount account)
        {
            string query = @"
        INSERT INTO BankAccount 
        (BankName, Country, AccountHolder, SwiftCode, AccountType, BranchLocation, 
         AccountNumber, Nominee, OpeningBalance)
        VALUES 
        (@BankName, @Country, @AccountHolder, @SwiftCode, @AccountType, @BranchLocation, 
         @AccountNumber, @Nominee, @OpeningBalance)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BankName", account.BankName);
                    command.Parameters.AddWithValue("@Country", account.Country);
                    command.Parameters.AddWithValue("@AccountHolder", account.AccountHolder);
                    command.Parameters.AddWithValue("@SwiftCode", account.SwiftCode);
                    command.Parameters.AddWithValue("@AccountType", account.AccountType);
                    command.Parameters.AddWithValue("@BranchLocation", account.BranchLocation);
                    command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                    command.Parameters.AddWithValue("@Nominee", account.Nominee);
                    command.Parameters.AddWithValue("@OpeningBalance", account.OpeningBalance);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateBankAccount(BankAccount account)
        {
            string query = @"
        UPDATE BankAccount
        SET BankName = @BankName,
            Country = @Country,
            AccountHolder = @AccountHolder,
            SwiftCode = @SwiftCode,
            AccountType = @AccountType,
            BranchLocation = @BranchLocation,
            AccountNumber = @AccountNumber,
            Nominee = @Nominee,
            OpeningBalance = @OpeningBalance
        WHERE AccountID = @AccountID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountID", account.AccountID);
                    command.Parameters.AddWithValue("@BankName", account.BankName);
                    command.Parameters.AddWithValue("@Country", account.Country);
                    command.Parameters.AddWithValue("@AccountHolder", account.AccountHolder);
                    command.Parameters.AddWithValue("@SwiftCode", account.SwiftCode);
                    command.Parameters.AddWithValue("@AccountType", account.AccountType);
                    command.Parameters.AddWithValue("@BranchLocation", account.BranchLocation);
                    command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                    command.Parameters.AddWithValue("@Nominee", account.Nominee);
                    command.Parameters.AddWithValue("@OpeningBalance", account.OpeningBalance);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteBankAccount(int accountID)
        {
            string query = "DELETE FROM BankAccount WHERE AccountID = @AccountID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountID", accountID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
