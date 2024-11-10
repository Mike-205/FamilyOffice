using Family_Office.ViewModel;
using System.Windows.Controls;

namespace Family_Office.Views
{
    /// <summary>
    /// Interaction logic for FamilyMembersView.xaml
    /// </summary>
    public partial class FamilyMembersView : Page
    {
        public FamilyMembersView()
        {
            InitializeComponent();
            var viewModel = new FamilyMembersViewModel();
            DataContext = viewModel;
            System.Diagnostics.Debug.WriteLine("DataContext set to: " + DataContext);

        }
    }
}
