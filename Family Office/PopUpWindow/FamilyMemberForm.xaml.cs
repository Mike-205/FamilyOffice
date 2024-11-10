using Family_Office.ViewModel;

using System.Windows;


namespace Family_Office.PopUpWindow
{
    /// <summary>
    /// Interaction logic for FamilyMemberForm.xaml
    /// </summary>
    public partial class FamilyMemberForm : Window
    {
        public FamilyMemberForm()
        {
            InitializeComponent();
            DataContext = new FamilyMemberMasterViewModel();
        }
    }
}
