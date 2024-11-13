using Family_Office.ViewModels;
using System.Windows.Controls;

namespace Family_Office.pages
{
    /// <summary>
    /// Interaction logic for GoalSettingsPage.xaml
    /// </summary>
    public partial class GoalSettingsPage : Page
    {
        public GoalSettingsPage()
        {
            InitializeComponent();
            DataContext = new GoalsSettingsViewModel();
        }
    }
}
