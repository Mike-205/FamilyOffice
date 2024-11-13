using Family_Office.DataAccess;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Family_Office.Models;
using System.IO;
using Family_Office.PopUpWindow;
using System.Diagnostics;

public class InvestmentViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class InvestmentPageViewModel : InvestmentViewModel
{
    private ObservableCollection<InvestmentListItem> _investmentsCollection;
    private string _filterText;
    private bool _isDataLoading = false;
    private bool _noDataAvailable = false;

    public InvestmentPageViewModel()
    {
        InitializeCommands();
        LoadInvestments();
    }

    public ObservableCollection<InvestmentListItem> InvestmentsCollection
    {
        get => _investmentsCollection;
        set
        {
            _investmentsCollection = value;
            OnPropertyChanged(nameof(InvestmentsCollection));
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

    // Commands
    public ICommand AddGoldCommand { get; private set; }
    public ICommand AddPropertyCommand { get; private set; }
    public ICommand EditInvestmentCommand { get; private set; }
    public ICommand RemoveInvestmentCommand { get; private set; }

    private void InitializeCommands()
    {
        AddGoldCommand = new RelayCommand5(ExecuteAddGold);
        AddPropertyCommand = new RelayCommand5(ExecuteAddProperty);
        EditInvestmentCommand = new RelayCommand6<InvestmentListItem>(ExecuteEditInvestment);
        RemoveInvestmentCommand = new RelayCommand6<InvestmentListItem>(ExecuteRemoveInvestment);
    }

    private async void LoadInvestments()
    {
        IsDataLoading = true;
        try
        {
            // Load gold investments with error handling
            List<InvestmentListItem> goldListItems = new List<InvestmentListItem>();
            try
            {
                var goldInvestments = GoldInvestmentDataAccess.GetGoldInvestments();
                goldListItems = goldInvestments
                    .Where(g => g != null) // Filter out any null records
                    .Select(g => new InvestmentListItem
                    {
                        Id = g.GoldInvestmentID,
                        InvestmentType = "Gold",
                        Type = g.GoldType ?? "Unknown Type", // Provide default value if null
                        IsGoldInvestment = true
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading gold investments: {ex.Message}");
                // Continue execution to load property investments
            }

            // Load property investments with error handling
            List<InvestmentListItem> propertyListItems = new List<InvestmentListItem>();
            try
            {
                var propertyInvestments = PropertyInvestmentDataAccess.GetPropertyInvestments();
                propertyListItems = propertyInvestments
                    .Where(p => p != null) // Filter out any null records
                    .Select(p => new InvestmentListItem
                    {
                        Id = p.PropertyInvestmentID,
                        InvestmentType = "Property",
                        Type = p.PropertyType ?? "Unknown Type", // Provide default value if null
                        IsGoldInvestment = false
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading property investments: {ex.Message}");
                // Continue execution to combine available data
            }

            // Combine available data
            var combinedList = goldListItems.Concat(propertyListItems).ToList();
            InvestmentsCollection = new ObservableCollection<InvestmentListItem>(combinedList);
            NoDataAvailable = !InvestmentsCollection.Any();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading investments: {ex.Message}", "Error",
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
            LoadInvestments();
            return;
        }

        var filteredInvestments = InvestmentsCollection.Where(i =>
            i.InvestmentType.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
            i.Type.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
        );

        InvestmentsCollection = new ObservableCollection<InvestmentListItem>(filteredInvestments);
        NoDataAvailable = !InvestmentsCollection.Any();
    }

    private void ExecuteAddGold()
    {
        try
        {
            Debug.WriteLine("Starting ExecuteAddGold");

            var masterWindow = new GoldInvestmentForm
            {
                Title = "Add Gold Investment",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 600,
                ResizeMode = ResizeMode.CanResize
            };
            Debug.WriteLine("GoldInvestmentForm window created");

            var viewModel = new GoldInvestmentViewModel();
            Debug.WriteLine("GoldInvestmentViewModel created");

            masterWindow.DataContext = viewModel;
            Debug.WriteLine("DataContext set");

            viewModel.SaveGoldInvCompleted += (s, e) =>
            {
                Debug.WriteLine("SaveGoldInvCompleted event triggered");
                try
                {
                    masterWindow?.Close();
                    Debug.WriteLine("Window closed");
                    LoadInvestments();
                    Debug.WriteLine("Investments reloaded");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in SaveGoldInvCompleted event handler: {ex.Message}");
                    MessageBox.Show($"Error after saving: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            Debug.WriteLine("Showing GoldInvestmentForm");
            masterWindow.ShowDialog(); // Add this line - it was missing!
            Debug.WriteLine("GoldInvestmentForm dialog closed");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in ExecuteAddGold: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            MessageBox.Show($"Error opening Gold Investment form: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ExecuteAddProperty()
    {
        try
        {
            Debug.WriteLine("Starting ExecuteAddProperty");

            var masterWindow = new PropertyInvestmentForm
            {
                Title = "Add Property Investment",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 600,
                ResizeMode = ResizeMode.CanResize
            };
            Debug.WriteLine("PropertyInvestmentForm window created");

            var viewModel = new PropertyInvestmentViewModel();
            Debug.WriteLine("PropertyInvestmentViewModel created");

            masterWindow.DataContext = viewModel;
            Debug.WriteLine("DataContext set");

            viewModel.SavePropertyInvCompleted += (s, e) =>
            {
                Debug.WriteLine("SavePropertyInvCompleted event triggered");
                try
                {
                    masterWindow?.Close();
                    Debug.WriteLine("Window closed");
                    LoadInvestments();
                    Debug.WriteLine("Investments reloaded");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in SavePropertyInvCompleted event handler: {ex.Message}");
                    MessageBox.Show($"Error after saving: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            Debug.WriteLine("Showing PropertyInvestmentForm");
            masterWindow.ShowDialog(); // Add this line - it was missing!
            Debug.WriteLine("PropertyInvestmentForm dialog closed");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in ExecuteAddProperty: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            MessageBox.Show($"Error opening Property Investment form: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ExecuteEditInvestment(InvestmentListItem investment)
    {
        if (investment.IsGoldInvestment)
        {
            var masterWindow = new GoldInvestmentForm
            {
                Title = "Edit Gold Investment",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 600,
                ResizeMode = ResizeMode.CanResize
            };

            // First get the full gold investment data
            var goldInvestment = GoldInvestmentDataAccess.GetGoldInvestments()
                .FirstOrDefault(g => g.GoldInvestmentID == investment.Id);

            if (goldInvestment == null)
            {
                MessageBox.Show("Unable to find the gold investment details.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var viewModel = new GoldInvestmentViewModel(goldInvestment);
            masterWindow.DataContext = viewModel;

            viewModel.SaveGoldInvCompleted += (s, e) =>
            {
                masterWindow?.Close();
                LoadInvestments(); // Refresh the list after editing
            };

            masterWindow.ShowDialog();
        }
        else
        {
            var masterWindow = new PropertyInvestmentForm
            {
                Title = "Edit Property Investment",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 600,
                ResizeMode = ResizeMode.CanResize
            };

            // First get the full property investment data
            var propertyInvestment = PropertyInvestmentDataAccess.GetPropertyInvestments()
                .FirstOrDefault(p => p.PropertyInvestmentID == investment.Id);

            if (propertyInvestment == null)
            {
                MessageBox.Show("Unable to find the property investment details.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var viewModel = new PropertyInvestmentViewModel(propertyInvestment);
            masterWindow.DataContext = viewModel;

            viewModel.SavePropertyInvCompleted += (s, e) =>
            {
                masterWindow?.Close();
                LoadInvestments(); // Refresh the list after editing
            };

            masterWindow.ShowDialog();
        }
    }

    private void ExecuteRemoveInvestment(InvestmentListItem investment)
    {
        var result = MessageBox.Show(
            $"Are you sure you want to delete this {investment.InvestmentType} investment?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                if (investment.IsGoldInvestment)
                {
                    GoldInvestmentDataAccess.DeleteGoldInvestment(investment.Id);
                }
                else
                {
                    // Implement when PropertyInvestment is ready
                    PropertyInvestmentDataAccess.DeletePropertyInvestment(investment.Id);
                }
                LoadInvestments(); // Refresh the list after deletion
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting investment: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

public class GoldInvestmentViewModel : InvestmentViewModel
{
    private GoldInvestment _goldInvestmentCollection;
    private bool _isNewInvestment;
    private ObservableCollection<string> _goldTypes;
    private ObservableCollection<string> _countries;
    private ObservableCollection<string> _storageLocations;
    private ObservableCollection<string> _inCareOf;
    private ObservableCollection<string> _currencies;
    private ObservableCollection<string> _purchaseVia;
    private byte[] _selectedDocument;

    public event EventHandler SaveGoldInvCompleted;

    public GoldInvestmentViewModel(GoldInvestment investment = null)
    {
        _isNewInvestment = investment == null;
        GInvestment = investment ?? new GoldInvestment();

        InitializeCollections();
        InitializeCommands();
    }

    #region Properties

    public GoldInvestment GInvestment
    {
        get => _goldInvestmentCollection;
        set
        {
            _goldInvestmentCollection = value;
            OnPropertyChanged(nameof(GInvestment));
        }
    }
   
    public ObservableCollection<string> GoldTypes
    {
        get => _goldTypes;
        set
        {
            _goldTypes = value;
            OnPropertyChanged(nameof(GoldTypes));
        }
    }

    public ObservableCollection<string> Countries
    {
        get => _countries;
        set
        {
            _countries = value;
            OnPropertyChanged(nameof(Countries));
        }
    }

    public ObservableCollection<string> StorageLocations
    {
        get => _storageLocations;
        set
        {
            _storageLocations = value;
            OnPropertyChanged(nameof(StorageLocations));
        }
    }

    public ObservableCollection<string> InCareOf
    {
        get => _inCareOf;
        set
        {
            _inCareOf = value;
            OnPropertyChanged(nameof(_inCareOf));
        }
    }

    public ObservableCollection<string> Currencies
    {
        get => _currencies;
        set
        {
            _currencies = value;
            OnPropertyChanged(nameof(Currencies));
        }
    }

    public ObservableCollection<string> PurchasedVia
    {
        get => _purchaseVia;
        set
        {
            _purchaseVia = value;
            OnPropertyChanged(nameof(PurchasedVia));
        }
    }

    #endregion

    #region Commands

    public ICommand SaveCommand { get; private set; }
    public ICommand ResetCommand { get; private set; }
    public ICommand UploadDocumentCommand { get; private set; }

    private void InitializeCommands()
    {
        SaveCommand = new RelayCommand5(ExecuteSave, CanExecuteSave);
        ResetCommand = new RelayCommand5(ExecuteReset);
        UploadDocumentCommand = new RelayCommand5(ExecuteUploadDocument);
    }

    #endregion

    #region Command Implementations

    private bool CanExecuteSave()
    {
        return GInvestment != null &&
               !string.IsNullOrEmpty(GInvestment.GoldType) &&
               GInvestment.TotalGrams > 0 &&
               GInvestment.Carat > 0 &&
               GInvestment.TotalPerGram > 0 &&
               !string.IsNullOrEmpty(GInvestment.StorageLocation) &&
               !string.IsNullOrEmpty(GInvestment.Country) &&
               !string.IsNullOrEmpty(GInvestment.InCareOf) &&
               !string.IsNullOrEmpty(GInvestment.Currency);
    }


    private void ExecuteSave()
    {
        try
        {
            if (_isNewInvestment)
            {
                GoldInvestmentDataAccess.AddGoldInvestment(GInvestment);
                MessageBox.Show("Gold investment added successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                GoldInvestmentDataAccess.UpdateGoldInvestment(GInvestment);
                MessageBox.Show("Gold investment updated successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SaveGoldInvCompleted?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving gold investment: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ExecuteReset()
    {
        if (_isNewInvestment)
        {
            GInvestment = new GoldInvestment();
        }
        else
        {
            // Reload the original investment data
            var originalInvestment = GoldInvestmentDataAccess.GetGoldInvestments()
                .FirstOrDefault(g => g.GoldInvestmentID == GInvestment.GoldInvestmentID);
            if (originalInvestment != null)
            {
                GInvestment = originalInvestment;
            }
        }
    }

    private void ExecuteUploadDocument()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
            Title = "Select Supporting Document"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                _selectedDocument = File.ReadAllBytes(openFileDialog.FileName);
                GInvestment.Document = _selectedDocument;
                MessageBox.Show("Document uploaded successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading document: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    #endregion

    #region Private Methods

    private void InitializeCollections()
    {
        GoldTypes = new ObservableCollection<string>
        {
            "Physical",
            "Digital"
        };

        StorageLocations = new ObservableCollection<string>
        {
            "Home",
            "Bank Locker",
            "Vault"
        };

        Currencies = new ObservableCollection<string>
        {
            "USD",
            "EUR",
            "INR"
        };

        // These collections might be loaded from a database in a real application
        Countries = new ObservableCollection<string>
        {
            "United States",
            "United Kingdom",
            "Switzerland",
            "Singapore",
            "Germany"
        };

        InCareOf = new ObservableCollection<string>
        {
            // This should be populated with family members or entities
            "Self",
            "Trust",
            "Company"
        };

        PurchasedVia = new ObservableCollection<string>
        {
            "Bank",
            "Dealer",
            "Exchange",
            "Private Sale"
        };
    }

    #endregion
}

public class PropertyInvestmentViewModel : InvestmentViewModel
{
    private PropertyInvestment _propertyInvestmentCollection;
    private bool _isNewInvestment;
    private ObservableCollection<string> _propertyTypes;
    private ObservableCollection<string> _purpose;
    private ObservableCollection<string> _unitMeasurements;
    private ObservableCollection<string> _ownershipTypes;
    private ObservableCollection<string> _countries;
    private byte[] _selectedDocument;

    public event EventHandler SavePropertyInvCompleted;

    public PropertyInvestmentViewModel(PropertyInvestment investment = null)
    {
        _isNewInvestment = investment == null;
        PInvestment = investment ?? new PropertyInvestment();

        InitializeCollections();
        InitializeCommands();
    }

    #region Properties

    public PropertyInvestment PInvestment
    {
        get => _propertyInvestmentCollection;
        set
        {
            _propertyInvestmentCollection = value;
            OnPropertyChanged(nameof(PInvestment));
        }
    }

    public ObservableCollection<string> PropertyTypes
    {
        get => _propertyTypes;
        set
        {
            _propertyTypes = value;
            OnPropertyChanged(nameof(PropertyTypes));
        }
    }

    public ObservableCollection<string> Purpose
    {
        get => _purpose;
        set
        {
            _purpose = value;
            OnPropertyChanged(nameof(Purpose));
        }
    }

    public ObservableCollection<string> UnitMeasurements
    {
        get => _unitMeasurements;
        set
        {
            _unitMeasurements = value;
            OnPropertyChanged(nameof(UnitMeasurements));
        }
    }

    public ObservableCollection<string> OwnershipTypes
    {
        get => _ownershipTypes;
        set
        {
            _ownershipTypes = value;
            OnPropertyChanged(nameof(OwnershipTypes));
        }
    }

    public ObservableCollection<string> Countries
    {
        get => _countries;
        set
        {
            _countries = value;
            OnPropertyChanged(nameof(Countries));
        }
    }

    #endregion

    #region Commands

    public ICommand SaveCommand { get; private set; }
    public ICommand ResetCommand { get; private set; }
    public ICommand UploadDocumentCommand { get; private set; }

    private void InitializeCommands()
    {
        SaveCommand = new RelayCommand5(ExecuteSave, CanExecuteSave);
        ResetCommand = new RelayCommand5(ExecuteReset);
        UploadDocumentCommand = new RelayCommand5(ExecuteUploadDocument);
    }

    #endregion

    #region Command Implementations

    private bool CanExecuteSave()
    {
        return PInvestment != null &&
               !string.IsNullOrEmpty(PInvestment.PropertyType) &&
               !string.IsNullOrEmpty(PInvestment.Purpose) &&
               !string.IsNullOrEmpty(PInvestment.Country) &&
               !string.IsNullOrEmpty(PInvestment.Address) &&
               PInvestment.TotalArea > 0 &&
               PInvestment.TotalPurchasePrice > 0 &&
               !string.IsNullOrEmpty(PInvestment.Ownership);
    }

    private void ExecuteSave()
    {
        try
        {
            if (_isNewInvestment)
            {
                PropertyInvestmentDataAccess.AddPropertyInvestment(PInvestment);
                MessageBox.Show("Property investment added successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                PropertyInvestmentDataAccess.UpdatePropertyInvestment(PInvestment);
                MessageBox.Show("Property investment updated successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SavePropertyInvCompleted?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving property investment: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ExecuteReset()
    {
        if (_isNewInvestment)
        {
            PInvestment = new PropertyInvestment();
        }
        else
        {
            // Reload the original investment data
            var originalInvestment = PropertyInvestmentDataAccess.GetPropertyInvestments()
                .FirstOrDefault(p => p.PropertyInvestmentID == PInvestment.PropertyInvestmentID);
            if (originalInvestment != null)
            {
                PInvestment = originalInvestment;
            }
        }
    }

    private void ExecuteUploadDocument()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
            Title = "Select Supporting Document"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                _selectedDocument = File.ReadAllBytes(openFileDialog.FileName);
                PInvestment.Document = _selectedDocument;
                MessageBox.Show("Document uploaded successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading document: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    #endregion

    #region Private Methods

    private void InitializeCollections()
    {
        PropertyTypes = new ObservableCollection<string>
        {
            "Residential",
            "Commercial",
            "Industrial",
            "Land",
            "Mixed Use"
        };

        Purpose = new ObservableCollection<string>
        {
            "Investment",
            "Personal Use",
            "Rental",
            "Development",
            "Mixed Use"
        };

        UnitMeasurements = new ObservableCollection<string>
        {
            "Square Feet",
            "Square Meters",
            "Acres",
            "Hectares"
        };

        OwnershipTypes = new ObservableCollection<string>
        {
            "Full Ownership",
            "Joint Ownership",
            "Leasehold",
            "Trust",
            "Company Owned"
        };

        Countries = new ObservableCollection<string>
        {
            "United States",
            "United Kingdom",
            "Switzerland",
            "Singapore",
            "Germany"
        };
    }

    #endregion
}

// Helper class for list items
public class InvestmentListItem
{
    public int Id { get; set; }
    public string InvestmentType { get; set; }
    public string Type { get; set; }
    public string PurchaseDate { get; set; }
    public bool IsGoldInvestment { get; set; }
}

// Command implementation
public class RelayCommand5 : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand5(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
    public void Execute(object parameter) => _execute();
}

public class RelayCommand6<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool> _canExecute;

    public RelayCommand6(Action<T> execute, Func<T, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter) =>
        parameter is T typedParameter && (_canExecute?.Invoke(typedParameter) ?? true);

    public void Execute(object parameter)
    {
        if (parameter is T typedParameter)
        {
            _execute(typedParameter);
        }
    }
}