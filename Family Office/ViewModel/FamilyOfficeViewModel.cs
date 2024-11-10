using Family_Office.DataAccess;
using Family_Office.Models;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Family_Office.ViewModel
{
    public class FamilyOfficeViewModel : INotifyPropertyChanged
    {
        private FamilyOffice _currentFamilyOffice;
        private bool _isExistingOffice;
        private string _statusMessage;
        private BitmapImage _logoImage;

        public event PropertyChangedEventHandler PropertyChanged;

        public FamilyOfficeViewModel()
        {
            LoadFamilyOfficeData();
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        // Properties
        public bool HasExistingData => _isExistingOffice;

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public string FamilyOfficeName
        {
            get => _currentFamilyOffice?.FamilyOfficeName;
            set
            {
                if (_currentFamilyOffice == null)
                    _currentFamilyOffice = new FamilyOffice();
                _currentFamilyOffice.FamilyOfficeName = value;
                OnPropertyChanged(nameof(FamilyOfficeName));
            }
        }

        public BitmapImage LogoImage
        {
            get => _logoImage;
            set
            {
                _logoImage = value;
                OnPropertyChanged(nameof(LogoImage));
            }
        }

        public string EstablishmentDate
        {
            get => _currentFamilyOffice?.EstablishmentDate;
            set
            {
                if (_currentFamilyOffice == null)
                    _currentFamilyOffice = new FamilyOffice();
                _currentFamilyOffice.EstablishmentDate = value;
                OnPropertyChanged(nameof(EstablishmentDate));
            }
        }

        public string Vision
        {
            get => _currentFamilyOffice?.Vision;
            set
            {
                if (_currentFamilyOffice == null)
                    _currentFamilyOffice = new FamilyOffice();
                _currentFamilyOffice.Vision = value;
                OnPropertyChanged(nameof(Vision));
            }
        }

        public string HeadOfFamily
        {
            get => _currentFamilyOffice?.HeadOfFamily;
            set
            {
                if (_currentFamilyOffice == null)
                    _currentFamilyOffice = new FamilyOffice();
                _currentFamilyOffice.HeadOfFamily = value;
                OnPropertyChanged(nameof(HeadOfFamily));
            }
        }

        public string ChiefInvestOfficer
        {
            get => _currentFamilyOffice?.ChiefInvestOfficer;
            set
            {
                if (_currentFamilyOffice == null)
                    _currentFamilyOffice = new FamilyOffice();
                _currentFamilyOffice.ChiefInvestOfficer = value;
                OnPropertyChanged(nameof(ChiefInvestOfficer));
            }
        }

        public string Motto
        {
            get => _currentFamilyOffice?.Motto;
            set
            {
                if (_currentFamilyOffice == null)
                    _currentFamilyOffice = new FamilyOffice();
                _currentFamilyOffice.Motto = value;
                OnPropertyChanged(nameof(Motto));
            }
        }

        public string Purpose
        {
            get => _currentFamilyOffice?.Purpose;
            set
            {
                if (_currentFamilyOffice == null)
                    _currentFamilyOffice = new FamilyOffice();
                _currentFamilyOffice.Purpose = value;
                OnPropertyChanged(nameof(Purpose));
            }
        }

        // Commands
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        // Methods
        private void LoadFamilyOfficeData()
        {
            var familyOffices = FamilyOfficeDataAccess.GetFamilyOffices();
            if (familyOffices.Any())
            {
                _currentFamilyOffice = familyOffices.First();
                _isExistingOffice = true;

                if (_currentFamilyOffice.FamilyOfficeLogo != null)
                {
                    LoadLogoImage(_currentFamilyOffice.FamilyOfficeLogo);
                }

                StatusMessage = "Existing family office data loaded.";
            }
            else
            {
                _currentFamilyOffice = new FamilyOffice();
                _isExistingOffice = false;
                StatusMessage = "No existing family office found. Please enter new office details.";
            }

            // Notify all properties have changed
            OnPropertyChanged(string.Empty);
        }

        private void LoadLogoImage(byte[] logoData)
        {
            if (logoData == null || logoData.Length == 0) return;

            try
            {
                var image = new BitmapImage();
                using (var mem = new MemoryStream(logoData))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                LogoImage = image;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading logo: {ex.Message}";
            }
        }

        private bool CanExecuteSave()
        {
            // Add validation logic here
            return !string.IsNullOrWhiteSpace(FamilyOfficeName);
        }

        private void ExecuteSave()
        {
            try
            {
                if (_isExistingOffice)
                {
                    FamilyOfficeDataAccess.UpdateFamilyOffice(_currentFamilyOffice);
                    StatusMessage = "Family office updated successfully.";
                }
                else
                {
                    FamilyOfficeDataAccess.AddFamilyOffice(_currentFamilyOffice);
                    _isExistingOffice = true;
                    StatusMessage = "New family office added successfully.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error saving family office: {ex.Message}";
            }
        }

        private void ExecuteCancel()
        {
            LoadFamilyOfficeData(); // Reload the original data
            StatusMessage = "Changes cancelled.";
        }

        public void UpdateLogo(byte[] logoData)
        {
            if (_currentFamilyOffice == null)
                _currentFamilyOffice = new FamilyOffice();

            _currentFamilyOffice.FamilyOfficeLogo = logoData;
            LoadLogoImage(logoData);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Basic RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
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
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}