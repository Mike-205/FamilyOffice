using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{
    public class GoalDataAccess
    {
        private static string ConnectionString = "Data Source=example.db;Version=3;";

        public static List<Goal> GetGoals()
        {
            List<Goal> goals = new List<Goal>();

            string query = "SELECT * FROM Goals";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        goals.Add(new Goal
                        {
                            GoalID = reader.GetInt32(0),
                            VisionYear = reader.GetInt32(1),
                            TargetAmount = reader.GetDouble(2),
                            RateOfCompounding = reader.GetDouble(3)
                        });
                    }
                }
            }

            return goals;
        }

        public static void AddGoal(Goal goal)
        {
            string query = @"
        INSERT INTO Goals (VisionYear, TargetAmount, RateOFCompounding)
        VALUES (@VisionYear, @TargetAmount, @RateOFCompounding)";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VisionYear", goal.VisionYear);
                    command.Parameters.AddWithValue("@TargetAmount", goal.TargetAmount);
                    command.Parameters.AddWithValue("@RateOFCompounding", goal.RateOfCompounding);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateGoal(Goal goal)
        {
            string query = @"
        UPDATE Goals
        SET VisionYear = @VisionYear,
            TargetAmount = @TargetAmount,
            RateOFCompounding = @RateOFCompounding
        WHERE GoalID = @GoalID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GoalID", goal.GoalID);
                    command.Parameters.AddWithValue("@VisionYear", goal.VisionYear);
                    command.Parameters.AddWithValue("@TargetAmount", goal.TargetAmount);
                    command.Parameters.AddWithValue("@RateOFCompounding", goal.RateOfCompounding);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteGoal(int goalID)
        {
            string query = "DELETE FROM Goals WHERE GoalID = @GoalID";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GoalID", goalID);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
