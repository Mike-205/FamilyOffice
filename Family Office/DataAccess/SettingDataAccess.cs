using Family_Office.Models;
using System.Data.SQLite;

namespace Family_Office.DataAccess
{

    public class SettingDataAccess
    {
        private static string ConnectionString = "Data Source=example.db;Version=3;";

        public static Settings GetSettings()
        {
            Settings settings = new Settings();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Settings LIMIT 1", connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        settings.settingsID = reader.GetInt32(0);
                        settings.DateFormat = reader.GetString(1);
                        settings.BaseCurrency = reader.GetString(2);
                        settings.ShowExchangeRatesInList = reader.GetBoolean(3);
                        settings.ShowBaseCurrencyEquivalent = reader.GetBoolean(4);
                        settings.UseThousandsSeparator = reader.GetBoolean(5);
                        settings.DecimalPlaces = reader.GetInt32(6);
                        settings.FontFamily = reader.GetString(7);
                        settings.BodyFontSize = reader.GetInt32(8);
                        settings.HeaderFontSize = reader.GetInt32(9);
                        settings.PrimaryColor = reader.GetString(10);
                        settings.SecondaryColor = reader.GetString(11);
                        settings.BackgroundColor = reader.GetString(12);
                        settings.HighContrast = reader.GetBoolean(13);
                        settings.LargeTextMode = reader.GetBoolean(14);
                        settings.Scale = reader.GetInt32(15);
                        settings.MarginSize = reader.GetInt32(16);
                        settings.BorderWidth = reader.GetDouble(17);
                        settings.BackgroundImage = reader.GetString(18);
                        settings.EnableHoverEffects = reader.GetBoolean(19);
                        settings.ShowFocusIndication = reader.GetBoolean(20);
                        settings.UseAnimation = reader.GetBoolean(21);
                    }
                }
            }
            return settings;
        }

        public static void UpdateSettings(Settings settings)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(@"
                    UPDATE Settings SET 
                        DateFormat = @DateFormat, 
                        BaseCurrency = @BaseCurrency, 
                        ShowExchangeRatesInList = @ShowExchangeRatesInList, 
                        ShowBaseCurrencyEquivalent = @ShowBaseCurrencyEquivalent,
                        UseThousandsSeparator = @UseThousandsSeparator, 
                        DecimalPlaces = @DecimalPlaces, 
                        FontFamily = @FontFamily, 
                        BodyFontSize = @BodyFontSize, 
                        HeaderFontSize = @HeaderFontSize, 
                        PrimaryColor = @PrimaryColor, 
                        SecondaryColor = @SecondaryColor, 
                        BackgroundColor = @BackgroundColor, 
                        HighContrast = @HighContrast, 
                        LargeTextMode = @LargeTextMode, 
                        Scale = @Scale, 
                        MarginSize = @MarginSize, 
                        BorderWidth = @BorderWidth, 
                        BackgroundImage = @BackgroundImage, 
                        EnableHoverEffects = @EnableHoverEffects, 
                        ShowFocusIndication = @ShowFocusIndication, 
                        UseAnimation = @UseAnimation
                    WHERE SettingID = @SettingID", connection);

                command.Parameters.AddWithValue("@DateFormat", settings.DateFormat);
                command.Parameters.AddWithValue("@BaseCurrency", settings.BaseCurrency);
                command.Parameters.AddWithValue("@ShowExchangeRatesInList", settings.ShowExchangeRatesInList ? 1 : 0);
                command.Parameters.AddWithValue("@ShowBaseCurrencyEquivalent", settings.ShowBaseCurrencyEquivalent ? 1 : 0);
                command.Parameters.AddWithValue("@UseThousandsSeparator", settings.UseThousandsSeparator ? 1 : 0);
                command.Parameters.AddWithValue("@DecimalPlaces", settings.DecimalPlaces);
                command.Parameters.AddWithValue("@FontFamily", settings.FontFamily);
                command.Parameters.AddWithValue("@BodyFontSize", settings.BodyFontSize);
                command.Parameters.AddWithValue("@HeaderFontSize", settings.HeaderFontSize);
                command.Parameters.AddWithValue("@PrimaryColor", settings.PrimaryColor);
                command.Parameters.AddWithValue("@SecondaryColor", settings.SecondaryColor);
                command.Parameters.AddWithValue("@BackgroundColor", settings.BackgroundColor);
                command.Parameters.AddWithValue("@HighContrast", settings.HighContrast ? 1 : 0);
                command.Parameters.AddWithValue("@LargeTextMode", settings.LargeTextMode ? 1 : 0);
                command.Parameters.AddWithValue("@Scale", settings.Scale);
                command.Parameters.AddWithValue("@MarginSize", settings.MarginSize);
                command.Parameters.AddWithValue("@BorderWidth", settings.BorderWidth);
                command.Parameters.AddWithValue("@BackgroundImage", settings.BackgroundImage);
                command.Parameters.AddWithValue("@EnableHoverEffects", settings.EnableHoverEffects ? 1 : 0);
                command.Parameters.AddWithValue("@ShowFocusIndication", settings.ShowFocusIndication ? 1 : 0);
                command.Parameters.AddWithValue("@UseAnimation", settings.UseAnimation ? 1 : 0);
                command.Parameters.AddWithValue("@SettingID", settings.settingsID);

                command.ExecuteNonQuery();
            }
        }
    }
}
