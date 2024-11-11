using Family_Office.ViewModel;
using System.Windows.Controls;

namespace Family_Office.pages
{
    /// <summary>
    /// Interaction logic for ThirdParty.xaml
    /// </summary>
    public partial class ThirdPartyView : Page
    {
        public ThirdPartyView()
        {
            InitializeComponent();
            DataContext = new ThirdPartyViewModel();
        }
    }
}
