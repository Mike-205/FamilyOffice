using Family_Office.DataAccess;
using Family_Office.pages;
using System.Windows;

namespace Family_Office
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();
            DatabaseDebugHelper.VerifyTableCreation();
            DatabaseDebugHelper.PrintAllTables();
            CurrencyInitialization.InitializeBaseCurrencies();
            MainFrame.Navigate(new LoginPage());
        }
    }
}