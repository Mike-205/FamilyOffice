using Family_Office.ViewModel;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Family_Office.pages
{
    /// <summary>
    /// Interaction logic for FamilyOfficeMastersPage.xaml
    /// </summary>
    public partial class FamilyOfficeMastersPage : Page
    {
        private FamilyOfficeViewModel ViewModel => DataContext as FamilyOfficeViewModel;

        public FamilyOfficeMastersPage()
        {
            InitializeComponent();
            DataContext = new FamilyOfficeViewModel();
        }

        private void BtnUploadLogo_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                Title = "Select Logo Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    byte[] logoData = File.ReadAllBytes(openFileDialog.FileName);
                    ViewModel?.UpdateLogo(logoData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}