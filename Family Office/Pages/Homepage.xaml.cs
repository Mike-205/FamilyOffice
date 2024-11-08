using System.Windows.Controls;

namespace Family_Office.pages
{
    public partial class Homepage : Page
    {
        public Homepage()
        {
            InitializeComponent();
            LoadDefaultPage();
        }

        private void LoadDefaultPage()
        {
            MainContentFrame.Navigate(new FamilyMembersMastersPage());
        }
    }
}