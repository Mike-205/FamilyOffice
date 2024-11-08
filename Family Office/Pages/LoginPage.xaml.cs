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
        private readonly string connectionString = "Data Source=Family Office.db;Version=3;";
        private byte[]? profileImageData;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void ProfilePicture_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadProfilePicture();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string hashedPassword = HashPassword(password);

            if (AuthenticateUser(username, hashedPassword))
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
            string username = txtUsername.Text; // Assuming username is set or input is handled before this

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ProfilePicture FROM Users WHERE Username = @Username";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(0))
                            {
                                byte[] profileImageBytes = (byte[])reader["ProfilePicture"];

                                // Load image from byte array and set it as the source for imgProfilePicture
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
                                // No profile picture found, display "Click to add profile" hint
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
                MessageBox.Show($"An error occurred while loading the profile picture: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool AuthenticateUser(string username, string hashedPassword)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Id FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

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
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        private void UpdateProfilePictureIfNeeded(SQLiteConnection connection, int userId)
        {
            if (profileImageData != null)
            {
                string updateQuery = "UPDATE Users SET ProfilePicture = @ProfilePicture WHERE Id = @Id";
                using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@ProfilePicture", profileImageData);
                    updateCommand.Parameters.AddWithValue("@Id", userId);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        private void NavigateToHomePage()
        {
            Homepage homePage = new Homepage();
            NavigationService?.Navigate(homePage);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
