using Family_Office.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Family_Office.pages
{
    public partial class Homepage : Page
    {
        public Homepage()
        {
            InitializeComponent();
            LoadDefaultPage();
        }

        private void LoadDefaultPage()
        {
            MainContentFrame.Navigate(new FamilyOfficeMastersPage());
        }

        private void NavigateToSection(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var stackPanel = button.Content as StackPanel;
                var textBlock = stackPanel.Children[1] as TextBlock;
                string sectionName = textBlock.Text;

                Page targetPage = null;

                // Navigate to the appropriate page based on the section name
                switch (sectionName)
                {
                    case "Family Office Masters":
                        targetPage = new FamilyOfficeMastersPage();
                        break;
                    case "Settings":
                        targetPage = new GeneralSettingsPage();
                        break;
                    case "Investments/Assets Master":
                        targetPage = new Investments();
                        break;
                    case "Family Members Masters":
                        targetPage = new FamilyMembersView();
                        break;

                    case "Bank Management":
                        targetPage = new BankAccountPage();
                        break;
                    case "Third Party":
                        targetPage = new ThirdParty();
                        break;
                }

                if (targetPage != null)
                {
                    try
                    {
                        MainContentFrame.Navigate(targetPage);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Navigation Error: {ex.Message}");
                    }

                }
            }
        }
    }
}