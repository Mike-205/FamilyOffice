using Family_Office.ViewModel;
using System.Windows.Controls;

namespace Family_Office.pages
{
    
    public partial class GeneralSettingsPage : Page
    {
        public GeneralSettingsPage()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}
