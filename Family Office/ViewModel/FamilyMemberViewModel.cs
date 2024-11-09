using Family_Office.DataAccess;
using Family_Office.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Family_Office.ViewModel
{
    public class FamilyMemberViewModel : INotifyPropertyChanged
    {
        private FamilyMember _selectedFamilyMember;
        public FamilyMember SelectedFamilyMember
        {
            get { return _selectedFamilyMember; }
            set { _selectedFamilyMember = value; OnPropertyChanged(nameof(SelectedFamilyMember)); }
        }

        private ObservableCollection<FamilyMember> _familyMembers;
        public ObservableCollection<FamilyMember> FamilyMembers
        {
            get { return _familyMembers; }
            set { _familyMembers = value; OnPropertyChanged(nameof(FamilyMembers)); }
        }

        public ICommand AddMemberCommand { get; }
        public ICommand UpdateMemberCommand { get; }

        public FamilyMemberViewModel()
        {
            FamilyMembers = new ObservableCollection<FamilyMember>(FamilyMemberDataAccess.GetFamilyMembers());
            AddMemberCommand = new RelayCommand(AddFamilyMember);
            UpdateMemberCommand = new RelayCommand(UpdateFamilyMember);
        }

        private void AddFamilyMember()
        {
            FamilyMemberDataAccess.AddFamilyMember(SelectedFamilyMember);
            FamilyMembers.Add(SelectedFamilyMember);
        }

        private void UpdateFamilyMember()
        {
            FamilyMemberDataAccess.UpdateFamilyMember(SelectedFamilyMember);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
