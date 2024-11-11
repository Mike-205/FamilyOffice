using Family_Office.ViewModel;
using System.Windows;

namespace Family_Office.Views
{
    /// <summary>
    /// Interaction logic for ThirdPartyForm.xaml
    /// </summary>
    public partial class ThirdPartyForm : Window
    {
        public ThirdPartyForm()
        {
            InitializeComponent();
            DataContext = new ThirdPartyFormViewModel();
        }
    }
}
