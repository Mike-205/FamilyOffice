using Family_Office.DataAccess;
using Family_Office.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using Family_Office.ViewModels;

namespace Family_Office.ViewModel
{
    public class KeyValueItem<T>
    {
        public string DisplayName { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class SettingsViewModel : INotifyPropertyChanged
    {
        private Settings _currentSettings;
        private bool _isDirty;
        private KeyValueItem<string> _selectedBackgroundImage;
        private KeyValueItem<string> _selectedPrimaryColor;
        private KeyValueItem<string> _selectedBackgroundColor;

        private ExchangeRateManagementViewModel _exchangeRateManagementViewModel;

        // Constructor
        public SettingsViewModel()
        {
            System.Diagnostics.Debug.WriteLine("Started Initialization");
            _exchangeRateManagementViewModel = new ExchangeRateManagementViewModel();
            System.Diagnostics.Debug.WriteLine("Initialized ExchangeRate viewModel");
            InitializeCommands();
            LoadSettings();
            //ExecuteReset();
        }

        #region Properties

        public ExchangeRateManagementViewModel ExchangeRateManagement
        {
            get => _exchangeRateManagementViewModel;
            set
            {
                _exchangeRateManagementViewModel = value;
                OnPropertyChanged(nameof(ExchangeRateManagement));
            }
        }

        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                _isDirty = value;
                OnPropertyChanged();
            }
        }

        // Date Format
        public string SelectedDateFormat
        {
            get => _currentSettings.DateFormat;
            set
            {
                if (_currentSettings.DateFormat != value)
                {
                    _currentSettings.DateFormat = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        // Currency Settings
        public string SelectedBaseCurrency
        {
            get => _currentSettings.BaseCurrency;
            set
            {
                if (_currentSettings.BaseCurrency != value)
                {
                    _currentSettings.BaseCurrency = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowExchangeRatesInList
        {
            get => _currentSettings.ShowExchangeRatesInList;
            set
            {
                if (_currentSettings.ShowExchangeRatesInList != value)
                {
                    _currentSettings.ShowExchangeRatesInList = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowBaseCurrencyEquivalent
        {
            get => _currentSettings.ShowBaseCurrencyEquivalent;
            set
            {
                if (_currentSettings.ShowBaseCurrencyEquivalent != value)
                {
                    _currentSettings.ShowBaseCurrencyEquivalent = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseThousandsSeparator
        {
            get => _currentSettings.UseThousandsSeparator;
            set
            {
                if (_currentSettings.UseThousandsSeparator != value)
                {
                    _currentSettings.UseThousandsSeparator = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public int DecimalPlaces
        {
            get => _currentSettings.DecimalPlaces;
            set
            {
                if (_currentSettings.DecimalPlaces != value)
                {
                    _currentSettings.DecimalPlaces = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        // UI Settings
        public string SelectedFontFamily
        {
            get => _currentSettings.FontFamily;
            set
            {
                if (_currentSettings.FontFamily != value)
                {
                    _currentSettings.FontFamily = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public int BodyFontSize
        {
            get => _currentSettings.BodyFontSize;
            set
            {
                if (_currentSettings.BodyFontSize != value)
                {
                    _currentSettings.BodyFontSize = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public int HeaderFontSize
        {
            get => _currentSettings.HeaderFontSize;
            set
            {
                if (_currentSettings.HeaderFontSize != value)
                {
                    _currentSettings.HeaderFontSize = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public KeyValueItem<string> SelectedPrimaryColor
        {
            get => _selectedPrimaryColor;
            set
            {
                if (_selectedPrimaryColor != value)
                {
                    _selectedPrimaryColor = value;
                    _currentSettings.PrimaryColor = value?.Value; // Store the color value
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public KeyValueItem<string> SelectedBackgroundColor
        {
            get => _selectedBackgroundColor;
            set
            {
                if (_selectedPrimaryColor != value)
                {
                    _selectedBackgroundColor = value;
                    _currentSettings.BackgroundColor = value?.Value; // Store the color value
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        // Accessibility Settings
        public bool HighContrast
        {
            get => _currentSettings.HighContrast;
            set
            {
                if (_currentSettings.HighContrast != value)
                {
                    _currentSettings.HighContrast = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public bool LargeTextMode
        {
            get => _currentSettings.LargeTextMode;
            set
            {
                if (_currentSettings.LargeTextMode != value)
                {
                    _currentSettings.LargeTextMode = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public int Scale
        {
            get => _currentSettings.Scale;
            set
            {
                if (_currentSettings.Scale != value)
                {
                    _currentSettings.Scale = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        // Layout Settings
        public int MarginSize
        {
            get => _currentSettings.MarginSize;
            set
            {
                if (_currentSettings.MarginSize != value)
                {
                    _currentSettings.MarginSize = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public double BorderWidth
        {
            get => _currentSettings.BorderWidth;
            set
            {
                if (_currentSettings.BorderWidth != value)
                {
                    _currentSettings.BorderWidth = value;
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        public KeyValueItem<string> SelectedBackgroundImage
        {
            get => _selectedBackgroundImage;
            set
            {
                if (_selectedBackgroundImage != value)
                {
                    _selectedBackgroundImage = value;
                    _currentSettings.BackgroundImage = value?.Value; // Store the actual file path
                    IsDirty = true;
                    OnPropertyChanged();
                }
            }
        }

        // Collections for ComboBoxes
        public ObservableCollection<string> DateFormats { get; private set; }
        public ObservableCollection<string> Currencies { get; private set; }
        public ObservableCollection<string> FontFamilies { get; private set; }
        public ObservableCollection<int> BodyFontSizes { get; private set; }
        public ObservableCollection<int> HeaderFontSizes { get; private set; }
        public ObservableCollection<KeyValueItem<string>> PrimaryColors { get; private set; }
        public ObservableCollection<KeyValueItem<string>> BackgroundColors { get; private set; }
        public ObservableCollection<int> Scales { get; private set; }
        public ObservableCollection<KeyValueItem<string>> BackgroundImages { get; private set; }
        public ObservableCollection<double> BorderThickness { get; private set; }    

        #endregion

        #region Commands

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand2(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand2(ExecuteCancel);
            ResetCommand = new RelayCommand2(ExecuteReset);
        }

        private bool CanExecuteSave()
        {
            return true;
        }

        private void ExecuteSave()
        {
            try
            {
                Debug.WriteLine($"Saving settings with ID: {_currentSettings.settingsID}");
                Debug.WriteLine($"Current Settings state:");
                Debug.WriteLine($"FontFamily: {_currentSettings.FontFamily}");
                Debug.WriteLine($"PrimaryColor: {_currentSettings.PrimaryColor}");
                Debug.WriteLine($"BackgroundColor: {_currentSettings.BackgroundColor}");

                if (_currentSettings.settingsID == 0)
                {
                    // If settingsID is 0, we need to reload settings to get the correct ID
                    var dbSettings = SettingDataAccess.GetSettings();
                    _currentSettings.settingsID = dbSettings.settingsID;
                }

                SettingDataAccess.UpdateSettings(_currentSettings);
                IsDirty = false;
                MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving settings: {ex}");
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Create a separate method for default settings to avoid duplication
        private Settings CreateDefaultSettings()
        {
            var defaultSettings = new Settings
            {
                DateFormat = "DD/MM/YYYY",
                BaseCurrency = "USD - US Dollar",
                ShowExchangeRatesInList = true,
                ShowBaseCurrencyEquivalent = true,
                UseThousandsSeparator = true,
                DecimalPlaces = 2,
                FontFamily = "Arial",
                BodyFontSize = 12,
                HeaderFontSize = 24,
                PrimaryColor = "#000080",
                BackgroundColor = "#FFFFFF",
                SecondaryColor = "#FFFFFF",  // Add this line
                HighContrast = false,
                LargeTextMode = false,
                Scale = 100,
                MarginSize = 5,
                BorderWidth = 1.0,
                BackgroundImage = "/Images/backgroundImage1.jpg",
                EnableHoverEffects = true,
                ShowFocusIndication = true,
                UseAnimation = true
            };

            // Save to database to get an ID
            SettingDataAccess.UpdateSettings(defaultSettings);

            // Reload to get the ID
            return SettingDataAccess.GetSettings();
        }

        private void ExecuteCancel()
        {
            if (IsDirty)
            {
                var result = MessageBox.Show("Do you want to discard changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    LoadSettings();
                }
            }
        }

        private void ExecuteReset()
        {
            var result = MessageBox.Show("Do you want to reset all settings to default?", "Confirm Reset",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _currentSettings = CreateDefaultSettings();

                    // Save the default settings to the database
                    SettingDataAccess.UpdateSettings(_currentSettings);

                    // Update selected items
                    _selectedBackgroundImage = BackgroundImages.FirstOrDefault(b => b.Value == _currentSettings.BackgroundImage)
                                             ?? BackgroundImages.First();
                    _selectedPrimaryColor = PrimaryColors.FirstOrDefault(c => c.Value == _currentSettings.PrimaryColor)
                                          ?? PrimaryColors.First();
                    _selectedBackgroundColor = BackgroundColors.FirstOrDefault(c => c.Value == _currentSettings.BackgroundColor)
                                             ?? BackgroundColors.First();

                    IsDirty = false;
                    RefreshAllProperties();

                    MessageBox.Show("Settings have been reset to default values.", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error resetting settings: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region Helper Methods

        private void LoadSettings()
        {
            try
            {
                // Add debug message
                System.Diagnostics.Debug.WriteLine("Attempting to load settings from database...");

                // Try to load existing settings
                _currentSettings = SettingDataAccess.GetSettings();

                System.Diagnostics.Debug.WriteLine($"Settings loaded from DB: {(_currentSettings != null ? "Success" : "Null")}");

                // If no settings exist or if they're null, initialize with defaults
                if (_currentSettings == null)
                {
                    System.Diagnostics.Debug.WriteLine("Creating default settings...");
                    _currentSettings = CreateDefaultSettings();
                    // Save default settings to database
                    SettingDataAccess.UpdateSettings(_currentSettings);
                    System.Diagnostics.Debug.WriteLine("Default settings saved to database.");
                }

                // Log current settings values
                System.Diagnostics.Debug.WriteLine($"Current Settings Values:");
                System.Diagnostics.Debug.WriteLine($"FontFamily: {_currentSettings.FontFamily}");
                System.Diagnostics.Debug.WriteLine($"BodyFontSize: {_currentSettings.BodyFontSize}");
                System.Diagnostics.Debug.WriteLine($"PrimaryColor: {_currentSettings.PrimaryColor}");

                // Initialize collections first
                PopulateComboBoxCollections();

                // Update selected items when loading settings
                _selectedBackgroundImage = BackgroundImages?.FirstOrDefault(b => b.Value == _currentSettings.BackgroundImage);
                _selectedPrimaryColor = PrimaryColors?.FirstOrDefault(c => c.Value == _currentSettings.PrimaryColor);
                _selectedBackgroundColor = BackgroundColors?.FirstOrDefault(c => c.Value == _currentSettings.BackgroundColor);

                System.Diagnostics.Debug.WriteLine($"Selected Primary Color: {_selectedPrimaryColor?.DisplayName ?? "null"}");
                System.Diagnostics.Debug.WriteLine($"Selected Background Image: {_selectedBackgroundImage?.DisplayName ?? "null"}");

                // Set defaults if null
                if (_selectedBackgroundImage == null && BackgroundImages.Any())
                {
                    _selectedBackgroundImage = BackgroundImages.First();
                    Debug.WriteLine($"Set default background image: {_selectedBackgroundImage.DisplayName}");
                }

                if (_selectedPrimaryColor == null && PrimaryColors.Any())
                {
                    _selectedPrimaryColor = PrimaryColors.First();
                    System.Diagnostics.Debug.WriteLine($"Set default primary color: {_selectedPrimaryColor.DisplayName}");
                }

                if (_selectedBackgroundColor == null && BackgroundColors.Any())
                {
                    _selectedBackgroundColor = BackgroundColors.First();
                    System.Diagnostics.Debug.WriteLine($"Set default background color: {_selectedBackgroundColor.DisplayName}");
                }

                IsDirty = false;
                RefreshAllProperties();
                Debug.WriteLine("Settings loading completed.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading settings: {ex}");
                MessageBox.Show($"Error loading settings: {ex.Message}\nDefault settings will be used.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                _currentSettings = CreateDefaultSettings();
                RefreshAllProperties();
            }
        }

        // Override the RefreshAllProperties to be more thorough
        private void RefreshAllProperties()
        {
            Debug.WriteLine("Refreshing all properties...");

            // Explicitly refresh each property
            OnPropertyChanged(nameof(SelectedFontFamily));
            OnPropertyChanged(nameof(BodyFontSize));
            OnPropertyChanged(nameof(HeaderFontSize));
            OnPropertyChanged(nameof(SelectedPrimaryColor));
            OnPropertyChanged(nameof(SelectedBackgroundColor));
            OnPropertyChanged(nameof(SelectedBackgroundImage));
            OnPropertyChanged(nameof(MarginSize));
            OnPropertyChanged(nameof(BorderWidth));
            OnPropertyChanged(nameof(SelectedDateFormat));
            OnPropertyChanged(nameof(SelectedBaseCurrency));
            OnPropertyChanged(nameof(ShowExchangeRatesInList));
            OnPropertyChanged(nameof(ShowBaseCurrencyEquivalent));
            OnPropertyChanged(nameof(UseThousandsSeparator));
            OnPropertyChanged(nameof(DecimalPlaces));
            OnPropertyChanged(nameof(HighContrast));
            OnPropertyChanged(nameof(LargeTextMode));
            OnPropertyChanged(nameof(Scale));

            Debug.WriteLine("Property refresh completed.");
        }

        private void PopulateComboBoxCollections()
        {
            if (DateFormats == null)
            {
                DateFormats = new ObservableCollection<string>
            {
                "DD/MM/YYYY",
                "MM/DD/YYYY",
                "YYYY-MM-DD"
            };
            }

            if (Currencies == null)
            {
                Currencies = new ObservableCollection<string>
            {
                "USD - US Dollar",
                "EUR - Euro",
                "GBP - British Pound",
                "JPY - Japanese Yen"
            };
            }
            
            if (FontFamilies == null)
            {
                FontFamilies = new ObservableCollection<string>
            {
                "Arial",
                "Helvetica",
                "Segoe UI"
            };
            }
            
            if (BodyFontSizes == null)
            {
                BodyFontSizes = new ObservableCollection<int>
            {
                10, 11, 12, 14, 16, 18
            };
            }
            
            if (HeaderFontSizes == null)
            {
                HeaderFontSizes = new ObservableCollection<int>
            {
                20, 22, 24, 26, 28
            };
            }

            if (BorderThickness == null)
            {
                BorderThickness = new ObservableCollection<double>
            {
                0.5, 1.0, 1.5, 2.0, 2.5
            };
            }

            if (PrimaryColors == null)
            {
                PrimaryColors = new ObservableCollection<KeyValueItem<string>>
            {
                new KeyValueItem<string> { DisplayName = "Navy Blue", Value = "#000080" },
                new KeyValueItem<string> { DisplayName = "Royal Blue", Value = "#4169E1" },
                new KeyValueItem<string> { DisplayName = "Steel Blue", Value = "#4682B4" }
            };
            }
            
            if (BackgroundColors == null)
            {
                BackgroundColors = new ObservableCollection<KeyValueItem<string>>
            {
                new KeyValueItem<string> { DisplayName = "White", Value = "#FFFFFF" },
                new KeyValueItem<string> { DisplayName = "Light Gray", Value = "#D3D3D3" },
                new KeyValueItem<string> { DisplayName = "Light Blue", Value = "#ADD8E6" }
            };
            }
            

            Scales = new ObservableCollection<int>
            {
                100, 125, 150
            };

            if(BackgroundImages == null)
            {
                BackgroundImages = new ObservableCollection<KeyValueItem<string>>
            {
                new KeyValueItem<string> { DisplayName = "Modern Business", Value = "/Images/backgroundImage1.jpg" },
                new KeyValueItem<string> { DisplayName = "Classic Finance", Value = "/Images/backgroundImage2.jpg" },
                new KeyValueItem<string> { DisplayName = "Corporate Theme", Value = "/Images/backgroundImage3.jpg" }
            };
            }

            _selectedBackgroundImage = BackgroundImages.FirstOrDefault(b => b.Value == _currentSettings.BackgroundImage);
            _selectedPrimaryColor = PrimaryColors.FirstOrDefault(c => c.Value == _currentSettings.PrimaryColor);
            _selectedBackgroundColor = BackgroundColors.FirstOrDefault(c => c.Value == _currentSettings.BackgroundColor);

            // Set defaults if no matches found

            if (_selectedBackgroundImage == null && BackgroundImages.Any())
                _selectedBackgroundImage = BackgroundImages.First();

            if (_selectedPrimaryColor == null && PrimaryColors.Any())
                _selectedPrimaryColor = PrimaryColors.First();

            if (_selectedBackgroundColor == null && BackgroundColors.Any())
                _selectedBackgroundColor = BackgroundColors.First();

        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    // Simple implementation of ICommand for the ViewModel
    public class RelayCommand2 : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand2(Action execute, Func<bool> canExecute = null)
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
}