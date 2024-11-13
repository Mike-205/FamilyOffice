using Family_Office.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.DataAccess
{
    public class DocumentDataAccess
    {
        public static string ConnectionString = "Data Source=example11.db;Version=3;";

        public static List<Document> GetDocuments()
        {
            List<Document> documents = new List<Document>();

            string query = "SELECT * FROM Documents";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        documents.Add(new Document
                        {
                            DocumentID = reader.GetInt32(0),
                            DocumentType = reader.GetString(1),
                            DocumentData = (byte[])reader["Document"]
                        });
                    }
                }
            }

            return documents;
        }

        public static void AddDocument(Document document)
        {
            string query = @"
        INSERT INTO Documents (DocumentType, Document)
        VALUES (@DocumentType, @Document)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DocumentType", document.DocumentType);
                    command.Parameters.AddWithValue("@Document", document.DocumentData);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateDocument(Document document)
        {
            string query = @"
        UPDATE Documents
        SET DocumentType = @DocumentType,
            Document = @Document
        WHERE DocumentID = @DocumentID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DocumentID", document.DocumentID);
                    command.Parameters.AddWithValue("@DocumentType", document.DocumentType);
                    command.Parameters.AddWithValue("@Document", document.DocumentData);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteDocument(int documentID)
        {
            string query = "DELETE FROM Documents WHERE DocumentID = @DocumentID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DocumentID", documentID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
