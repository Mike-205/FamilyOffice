using Family_Office.DataAccess;
using Family_Office.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Family_Office.PopUpWindow;

namespace Family_Office.ViewModel
{
    // Base ViewModel implementing INotifyPropertyChanged
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // ViewModel for the FamilyMembersView (List View)
    public class FamilyMembersViewModel : ViewModelBase
    {
        private ObservableCollection<FamilyMember> _familyMembers;
        private string _filterText;
        private bool _isDataLoading = false;
        private bool _noDataAvailable = false;

        public FamilyMembersViewModel()
        {
            LoadFamilyMembers();
            InitializeCommands();
        }

        public ObservableCollection<FamilyMember> FamilyMembers
        {
            get => _familyMembers;
            set
            {
                _familyMembers = value;
                OnPropertyChanged(nameof(FamilyMembers));
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                ApplyFilter();
            }
        }

        public bool IsDataLoading
        {
            get => _isDataLoading;
            set
            {
                _isDataLoading = value;
                OnPropertyChanged(nameof(IsDataLoading));
            }
        }

        public bool NoDataAvailable
        {
            get => _noDataAvailable;
            set
            {
                _noDataAvailable = value;
                OnPropertyChanged(nameof(NoDataAvailable));
            }
        }

        public ICommand AddMemberCommand { get; private set; }
        public ICommand EditMemberCommand { get; private set; }
        public ICommand RemoveMemberCommand { get; private set; }

        private void InitializeCommands()
        {
            AddMemberCommand = new RelayCommand3(ExecuteAddMember);
            EditMemberCommand = new RelayCommand4<FamilyMember>(ExecuteEditMember);
            RemoveMemberCommand = new RelayCommand4<FamilyMember>(ExecuteRemoveMember);
        }

        private async void LoadFamilyMembers()
        {
            IsDataLoading = true;
            try
            {
                var members = await Task.Run(() => FamilyMemberDataAccess.GetFamilyMembers());
                System.Diagnostics.Debug.WriteLine($"Loaded {members?.Count() ?? 0} members");

                // Debug each member's data
                foreach (var member in members)
                {
                    System.Diagnostics.Debug.WriteLine($"Member: {member.FullName}, Relation: {member.FamilyRelation}, Email: {member.PersonalEmail}");
                }

                FamilyMembers = new ObservableCollection<FamilyMember>(members);
                NoDataAvailable = !FamilyMembers.Any();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                MessageBox.Show($"Error loading family members: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsDataLoading = false;
            }
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                LoadFamilyMembers();
                return;
            }

            var filteredMembers = FamilyMembers.Where(m =>
                m.FullName.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                m.PersonalEmail.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            FamilyMembers = new ObservableCollection<FamilyMember>(filteredMembers);
        }

        private void ExecuteAddMember()
        {
            var masterWindow = new FamilyMemberForm
            {
                Title = "Add Family Member",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 600,
                ResizeMode = ResizeMode.CanResize
            };

            var masterVM = new FamilyMemberMasterViewModel();
            masterWindow.DataContext = masterVM;

            masterVM.SaveCompleted += (s, e) =>
            {
                LoadFamilyMembers();
                masterWindow?.Close();
            };

            masterWindow.ShowDialog(); // Using ShowDialog instead of Show for modal behavior
        }

        private void ExecuteEditMember(FamilyMember member)
        {
            var masterWindow = new FamilyMemberForm
            {
                Title = "Edit Family Member",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 600,
                ResizeMode = ResizeMode.CanResize
            };

            var masterVM = new FamilyMemberMasterViewModel(member);
            masterWindow.DataContext = masterVM;

            masterVM.SaveCompleted += (s, e) =>
            {
                LoadFamilyMembers();
                masterWindow?.Close();
            };

            masterWindow.ShowDialog(); // Using ShowDialog instead of Show for modal behavior
        }


        private async void ExecuteRemoveMember(FamilyMember member)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to remove {member.FullName}?",
                "Confirm Removal",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await Task.Run(() => FamilyMemberDataAccess.DeleteFamilyMember(member.FamilyMemberID));
                    LoadFamilyMembers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error removing family member: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    // ViewModel for the FamilyMembersMastersPage (Detail/Edit View)
    public class FamilyMemberMasterViewModel : ViewModelBase
    {
        private FamilyMember _member;
        private bool _isNewMember;
        private string _selectedImagePath;
        private ImageSource _profileImageSource;
        private ObservableCollection<string> _familyRelations;
        private ObservableCollection<string> _roles;
        private ObservableCollection<string> _statusOptions;
        private ObservableCollection<string> _educationStatusOptions;
        private ObservableCollection<string> _countryCodes;
        private ObservableCollection<string> _nationalities;

        public event EventHandler SaveCompleted;

        // Add this to the constructor
        public FamilyMemberMasterViewModel(FamilyMember member = null)
        {
            _isNewMember = member == null;
            Member = member ?? new FamilyMember();

            // Initialize the profile image if it exists
            if (!string.IsNullOrEmpty(Member.ProfilePicture))
            {
                UpdateProfileImage(Member.ProfilePicture);
            }

            InitializeCommands();
            LoadComboBoxData();
        }

        // Existing properties
        public FamilyMember Member
        {
            get => _member;
            set
            {
                _member = value;
                OnPropertyChanged(nameof(Member));
            }
        }

        // Add this new property
        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set
            {
                _profileImageSource = value;
                OnPropertyChanged(nameof(ProfileImageSource));
            }
        }

        public string SelectedImagePath
        {
            get => _selectedImagePath;
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged(nameof(SelectedImagePath));
                if (!string.IsNullOrEmpty(value))
                {
                    UpdateProfileImage(value);
                }
            }
        }

        private void UpdateProfileImage(string imagePath)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                ProfileImageSource = bitmap;
                Member.ProfilePicture = imagePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // New ComboBox Collections
        public ObservableCollection<string> FamilyRelations
        {
            get => _familyRelations;
            set
            {
                _familyRelations = value;
                OnPropertyChanged(nameof(FamilyRelations));
            }
        }

        public ObservableCollection<string> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }

        public ObservableCollection<string> StatusOptions
        {
            get => _statusOptions;
            set
            {
                _statusOptions = value;
                OnPropertyChanged(nameof(StatusOptions));
            }
        }

        public ObservableCollection<string> EducationStatusOptions
        {
            get => _educationStatusOptions;
            set
            {
                _educationStatusOptions = value;
                OnPropertyChanged(nameof(EducationStatusOptions));
            }
        }

        public ObservableCollection<string> CountryCodes
        {
            get => _countryCodes;
            set
            {
                _countryCodes = value;
                OnPropertyChanged(nameof(CountryCodes));
            }
        }

        public ObservableCollection<string> Nationalities
        {
            get => _nationalities;
            set
            {
                _nationalities = value;
                OnPropertyChanged(nameof(Nationalities));
            }
        }

        // Existing commands
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand UploadPhotoCommand { get; private set; }
        public ICommand UploadDocumentCommand { get; private set; }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand3(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand3(ExecuteCancel);
            UploadPhotoCommand = new RelayCommand3(ExecuteUploadPhoto);
            UploadDocumentCommand = new RelayCommand3(ExecuteUploadDocument);
        }

        private void LoadComboBoxData()
        {
            // Family Relations
            FamilyRelations = new ObservableCollection<string>
        {
            "Father", "Mother", "Son", "Daughter", "Spouse", "Sibling", "Grandparent", "Grandchild",
            "Uncle", "Aunt", "Cousin", "In-law", "Other"
        };

            // Roles
            Roles = new ObservableCollection<string>
        {
            "Head of Family", "Primary Contact", "Secondary Contact", "Member", "Dependent"
        };

            // Status Options
            StatusOptions = new ObservableCollection<string>
        {
            "Active", "Inactive", "Deceased", "Separated"
        };

            // Education Status
            EducationStatusOptions = new ObservableCollection<string>
        {
            "School", "Undergraduate", "Graduate", "Post Graduate", "Doctorate", "Other"
        };

            // Country Codes (Common ones, can be expanded)
            CountryCodes = new ObservableCollection<string>
        {
            "+1", "+44", "+91", "+971", "+966", "+65", "+81", "+86"
        };

            // Nationalities (Common ones, can be expanded)
            Nationalities = new ObservableCollection<string>
        {
            "American", "British", "Indian", "Emirati", "Saudi", "Singaporean", "Japanese", "Chinese",
            "Canadian", "Australian", "German", "French"
        };
        }

        private bool CanExecuteSave()
        {
            // Enhanced validation including new required fields
            return !string.IsNullOrWhiteSpace(Member.CustomMemberID) &&
                   !string.IsNullOrWhiteSpace(Member.FullName) &&
                   !string.IsNullOrWhiteSpace(Member.DOB) &&
                   !string.IsNullOrWhiteSpace(Member.FamilyRelation) &&
                   !string.IsNullOrWhiteSpace(Member.Role) &&
                   !string.IsNullOrWhiteSpace(Member.Status) &&
                   !string.IsNullOrWhiteSpace(Member.CountryCode1) &&
                   !string.IsNullOrWhiteSpace(Member.Telephone1) &&
                   !string.IsNullOrWhiteSpace(Member.PersonalEmail) &&
                   !string.IsNullOrWhiteSpace(Member.Nationality) &&
                   !string.IsNullOrWhiteSpace(Member.CurrentResidence);
                   
        }

        private async void ExecuteSave()
        {
            try
            {
                if (_isNewMember)
                {
                    await Task.Run(() => FamilyMemberDataAccess.AddFamilyMember(Member));
                    MessageBox.Show("Family Member Added Successfully!");
                }
                else
                {
                    await Task.Run(() => FamilyMemberDataAccess.UpdateFamilyMember(Member));
                    MessageBox.Show("Family Member Updated Successfully!");
                }

                SaveCompleted.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving family member: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancel()
        {
            var window = Application.Current.Windows.OfType<FamilyMemberForm>()
                .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private void ExecuteUploadPhoto()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedImagePath = dialog.FileName;
            }
        }

        private void ExecuteUploadDocument()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    Member.Document = File.ReadAllBytes(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading document: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    // Helper class for ICommand implementation
    public class RelayCommand3 : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand3(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }

    public class RelayCommand4<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand4(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}