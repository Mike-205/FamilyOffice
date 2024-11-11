using Family_Office.DataAccess;
using Family_Office.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.IO;
using Family_Office.Views;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Family_Office.ViewModel
{
    public class ThirdPartyViewModel : ViewModelBase
    {
        private ObservableCollection<ThirdParty> _thirdParties;
        private string _filterText;
        private bool _isDataLoading = false;
        private bool _noDataAvailable = false;

        public ThirdPartyViewModel()
        {
            LoadThirdParties();
            InitializeCommands();
        }

        public ObservableCollection<ThirdParty> ThirdParties
        {
            get => _thirdParties;
            set
            {
                _thirdParties = value;
                OnPropertyChanged(nameof(ThirdParties));
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

        public ICommand AddThirdPartyCommand { get; private set; }
        public ICommand EditThirdPartyCommand { get; private set; }
        public ICommand RemoveThirdPartyCommand { get; private set; }

        private void InitializeCommands()
        {
            AddThirdPartyCommand = new RelayCommand3(ExecuteAddMember);
            EditThirdPartyCommand = new RelayCommand4<ThirdParty>(ExecuteEditThirdParty);
            RemoveThirdPartyCommand = new RelayCommand4<ThirdParty>(ExecuteRemoveThirdParty);
        }

        private async void LoadThirdParties()
        {
            IsDataLoading = true;
            try
            {
                var parties = await Task.Run(() => ThirdPartyDataAccess.GetThirdParties());
                System.Diagnostics.Debug.WriteLine($"Loaded {parties?.Count() ?? 0} third parties");

                foreach (var party in parties)
                {
                    System.Diagnostics.Debug.WriteLine($"Party: {party.FullName}, Type: {party.PartyType}, Email: {party.EmailAddress}");
                }

                ThirdParties = new ObservableCollection<ThirdParty>(parties);
                NoDataAvailable = !ThirdParties.Any();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                MessageBox.Show($"Error loading third parties: {ex.Message}", "Error",
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
                LoadThirdParties();
                return;
            }

            var filteredParties = ThirdParties.Where(t =>
                t.FullName.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                t.EmailAddress.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                t.PartyID.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            ThirdParties = new ObservableCollection<ThirdParty>(filteredParties);
        }

        private void ExecuteAddMember()
        {
            var thirdPartyWindow = new ThirdPartyForm
            {
                Title = "Add Third Party",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 600,
                ResizeMode = ResizeMode.CanResize
            };

            var thirdPartyVM = new ThirdPartyFormViewModel();
            thirdPartyWindow.DataContext = thirdPartyVM;

            thirdPartyVM.SaveCompleted += (s, e) =>
            {
                LoadThirdParties();
                thirdPartyWindow?.Close();
            };

            thirdPartyWindow.ShowDialog();
        }

        private void ExecuteEditThirdParty(ThirdParty thirdParty)
        {
            var thirdPartyWindow = new ThirdPartyForm
            {
                Title = "Edit Third Party",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 600,
                ResizeMode = ResizeMode.CanResize
            };

            var thirdPartyVM = new ThirdPartyFormViewModel(thirdParty);
            thirdPartyWindow.DataContext = thirdPartyVM;

            thirdPartyVM.SaveCompleted += (s, e) =>
            {
                LoadThirdParties();
                thirdPartyWindow?.Close();
            };

            thirdPartyWindow.ShowDialog();
        }

        private async void ExecuteRemoveThirdParty(ThirdParty thirdParty)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to remove {thirdParty.FullName}?",
                "Confirm Removal",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await Task.Run(() => ThirdPartyDataAccess.DeleteThirdParty(thirdParty.ThirdPartyID));
                    LoadThirdParties();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error removing third party: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class ThirdPartyFormViewModel : ViewModelBase
    {
        private ThirdParty _thirdParty;
        private bool _isNewParty;
        private byte[] _selectedPhoto;
        private byte[] _selectedDocument;
        private ImageSource _photoImageSource;
        private ObservableCollection<string> _partyTypes;
        private ObservableCollection<string> _statusTypes;
        private ObservableCollection<string> _countryCodes;
        private ObservableCollection<string> _currencyTypes;

        public event EventHandler SaveCompleted;

        public ThirdPartyFormViewModel(ThirdParty thirdParty = null)
        {
            _isNewParty = thirdParty == null;
            ThirdParty = thirdParty ?? new ThirdParty();

            // Initialize photo if it exists
            if (ThirdParty.Photo != null && ThirdParty.Photo.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream(ThirdParty.Photo))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                        PhotoImageSource = image;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error loading existing photo: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }

            InitializeCommands();
            LoadComboBoxData();
        }

        public ImageSource PhotoImageSource
        {
            get => _photoImageSource;
            set
            {
                _photoImageSource = value;
                OnPropertyChanged(nameof(PhotoImageSource));
            }
        }
        public ThirdParty ThirdParty
        {
            get => _thirdParty;
            set
            {
                _thirdParty = value;
                OnPropertyChanged(nameof(ThirdParty));
            }
        }

        public ObservableCollection<string> PartyTypes
        {
            get => _partyTypes;
            set
            {
                _partyTypes = value;
                OnPropertyChanged(nameof(PartyTypes));
            }
        }

        public ObservableCollection<string> StatusTypes
        {
            get => _statusTypes;
            set
            {
                _statusTypes = value;
                OnPropertyChanged(nameof(StatusTypes));
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

        public ObservableCollection<string> CurrencyTypes
        {
            get => _currencyTypes;
            set
            {
                _currencyTypes = value;
                OnPropertyChanged(nameof(CurrencyTypes));
            }
        }

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
            PartyTypes = new ObservableCollection<string>
            {
                "Vendor", "Customer", "Consultant", "Employee", "Contractor", "Agent", "Other"
            };

            StatusTypes = new ObservableCollection<string>
            {
                "Active", "Inactive", "Suspended", "Terminated", "On Hold"
            };

            CountryCodes = new ObservableCollection<string>
            {
                "+1", "+44", "+91", "+971", "+966", "+65", "+81", "+86"
            };

            CurrencyTypes = new ObservableCollection<string>
            {
                "USD", "EUR", "GBP", "INR", "AED", "SAR", "SGD", "JPY", "CNY"
            };
        }

        private bool CanExecuteSave()
        {
            return true;
        }

        private async void ExecuteSave()
        {
            try
            {
                if (_isNewParty)
                {
                    await Task.Run(() => ThirdPartyDataAccess.AddThirdParty(ThirdParty));
                    MessageBox.Show("Third Party Added Successfully!");
                }
                else
                {
                    await Task.Run(() => ThirdPartyDataAccess.UpdateThirdParty(ThirdParty));
                    MessageBox.Show("Third Party Updated Successfully!");
                }

                SaveCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving third party: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancel()
        {
            var window = Application.Current.Windows.OfType<ThirdPartyForm>()
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
                try
                {
                    // Create BitmapImage
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(dialog.FileName, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    // Set the image source for display
                    PhotoImageSource = bitmap;

                    // Store the byte array for database
                    ThirdParty.Photo = File.ReadAllBytes(dialog.FileName);
                    MessageBox.Show("Photo uploaded successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading photo: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                    ThirdParty.Document = File.ReadAllBytes(dialog.FileName);
                    MessageBox.Show("Document uploaded successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading document: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}