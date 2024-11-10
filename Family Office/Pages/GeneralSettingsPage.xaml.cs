using Family_Office.ViewModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace Family_Office.pages
{
    
    public partial class GeneralSettingsPage : Page
    {
        private SettingsViewModel SettingsViewModel => DataContext as SettingsViewModel;
        public GeneralSettingsPage()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
            
        }
    }
}
