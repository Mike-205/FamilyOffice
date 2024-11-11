using Family_Office.Inteface;
using Family_Office.ViewModel;
using System.Diagnostics;

using System.Windows.Controls;

namespace Family_Office.pages
{
    /// <summary>
    /// Interaction logic for BankAccountPage.xaml
    /// </summary>
    public partial class BankAccountPage : Page
    {
        public BankAccountPage()
        {
            InitializeComponent();
            DataContext = new BankManagementViewModel(new SQLiteDataService());
        }

        private void BankNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
