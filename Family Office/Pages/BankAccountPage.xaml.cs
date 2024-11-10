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
            try
            {
                InitializeComponent();
            } catch (Exception ex)
            {
                Debug.WriteLine($"Settings Constructor Error: {ex.Message}");
            }
        }
    }
}
