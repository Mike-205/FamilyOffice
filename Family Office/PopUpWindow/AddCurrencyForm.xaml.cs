using Family_Office.ViewModel;
using Family_Office.ViewModels;
using System.Windows;

namespace Family_Office.Views
{
    /// <summary>
    /// Interaction logic for AddCurrecnyView.xaml
    /// </summary>
    public partial class AddCurrencyForm : Window
    {
        public AddCurrencyForm()
        {
            InitializeComponent();
            DataContext = new AddCurrencyFormViewModel();

        }
    }
}
