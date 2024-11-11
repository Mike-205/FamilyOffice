using Microsoft.Win32;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Family_Office.pages
{
    public partial class LoginPage : Page
    {
        private readonly string connectionString = "Data Source=example5.db;Version=3;";
        private byte[]? profileImageData;

        public LoginPage()
        {
            InitializeComponent();
            EnsureDefaultUserExists();
        }

        private void EnsureDefaultUserExists()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // First check if user exists
                    string checkQuery = "SELECT COUNT(*) FROM User WHERE Username = 'admin'";
                    using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                    {
                        int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (userCount == 0)
                        {
                            // Insert default user if none exists
                            string insertQuery = @"
                                INSERT INTO User (Username, Password) 
                                VALUES ('admin', '12345')";
                            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                            {
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization error: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProfilePicture_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadProfilePicture();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Add debug message
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string debugQuery = "SELECT * FROM User WHERE Username = @Username";
                    using (SQLiteCommand command = new SQLiteCommand(debugQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["Password"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Debug: User not found in database");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Debug error: {ex.Message}");
            }

            if (AuthenticateUser(username, password))
            {
                LoadUserProfilePicture();
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToHomePage();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.",
                              "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadUserProfilePicture()
        {
            string username = txtUsername.Text;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Profile FROM User WHERE Username = @Username";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(reader.GetOrdinal("Profile")))
                            {
                                byte[] profileImageBytes = (byte[])reader["Profile"];

                                BitmapImage bitmap = new BitmapImage();
                                using (MemoryStream ms = new MemoryStream(profileImageBytes))
                                {
                                    bitmap.BeginInit();
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.StreamSource = ms;
                                    bitmap.EndInit();
                                }

                                imgProfilePicture.Source = bitmap;
                                txtProfileHint.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                imgProfilePicture.Source = null;
                                txtProfileHint.Visibility = Visibility.Visible;
                                txtProfileHint.Text = "Click to add profile";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the profile picture: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadProfilePicture()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    imgProfilePicture.Source = bitmap;
                    txtProfileHint.Visibility = Visibility.Collapsed;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmap));
                        encoder.Save(ms);
                        profileImageData = ms.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}",
                                  "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT UserID FROM User WHERE Username = @Username AND Password = @Password";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                UpdateProfilePictureIfNeeded(connection, userId);
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        private void UpdateProfilePictureIfNeeded(SQLiteConnection connection, int userId)
        {
            if (profileImageData != null)
            {
                string updateQuery = "UPDATE User SET Profile = @Profile WHERE UserID = @UserID";
                using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Profile", profileImageData);
                    updateCommand.Parameters.AddWithValue("@UserID", userId);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        private void NavigateToHomePage()
        {
            Homepage homePage = new Homepage();
            NavigationService?.Navigate(homePage);
        }
    }
}