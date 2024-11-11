using System.Windows;

namespace Family_Office.PopUpWindow
{
    /// <summary>
    /// Interaction logic for PropertyInvestmentForm.xaml
    /// </summary>
    public partial class PropertyInvestmentForm : Window
    {
        public PropertyInvestmentForm()
        {
            InitializeComponent();
            DataContext = new PropertyInvestmentViewModel();
        }
    }
}
