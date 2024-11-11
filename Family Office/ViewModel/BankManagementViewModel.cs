using Family_Office.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Family_Office.ViewModel
{
    public class BankManagementViewModel : INotifyPropertyChanged
    {
        private readonly IDataService _dataService; // You'll need to implement this interface
        private BankAccount _selectedBankAccount;
        private CashAsset _selectedCashAsset;

        public BankManagementViewModel(IDataService dataService)
        {
            _dataService = dataService;

            // Initialize ObservableCollections for lists
            BankAccounts = new ObservableCollection<BankAccount>();
            CashAssets = new ObservableCollection<CashAsset>();

            // Initialize ComboBox Collections
            BankNames = new ObservableCollection<string>
            {
                "Chase",
                "Bank of America",
                "Wells Fargo",
                "Citibank",
                "HSBC",
                "Goldman Sachs",
                "Morgan Stanley",
                "Deutsche Bank"
            };

            AccountTypes = new ObservableCollection<string>
            {
                "Checking",
                "Savings",
                "Investment",
                "Money Market",
                "Certificate of Deposit",
                "Business Checking",
                "Business Savings"
            };

            Countries = new ObservableCollection<string>
            {
                "United States",
                "United Kingdom",
                "Canada",
                "Australia",
                "Germany",
                "France",
                "Switzerland",
                "Singapore",
                "Hong Kong",
                "Japan"
            };

            Currencies = new ObservableCollection<string>
            {
                "USD",
                "EUR",
                "GBP",
                "CAD",
                "AUD",
                "CHF",
                "JPY",
                "SGD",
                "HKD"
            };

            Locations = new ObservableCollection<string>
            {
                "United States",
                "United Kingdom",
                "Canada",
                "Australia",
                "Singapore",
                "Hong Kong",
                "Switzerland"
            };

            // Initialize Commands
            CreateAccountCommand = new RelayCommand8(ExecuteCreateAccount, CanExecuteCreateAccount);
            SaveChangesCommand = new RelayCommand8(ExecuteSaveChanges, CanExecuteSaveChanges);
            SaveCashEntryCommand = new RelayCommand8(ExecuteSaveCashEntry, CanExecuteSaveCashEntry);

            // Load initial data
            LoadDataAsync();
        }

        #region Properties

        // Main Collections
        public ObservableCollection<BankAccount> BankAccounts { get; private set; }
        public ObservableCollection<CashAsset> CashAssets { get; private set; }

        // ComboBox Collections
        public ObservableCollection<string> BankNames { get; private set; }
        public ObservableCollection<string> AccountTypes { get; private set; }
        public ObservableCollection<string> Countries { get; private set; }
        public ObservableCollection<string> Currencies { get; private set; }
        public ObservableCollection<string> Locations { get; private set; }


        public BankAccount SelectedBankAccount
        {
            get => _selectedBankAccount;
            set
            {
                if (_selectedBankAccount != value)
                {
                    _selectedBankAccount = value;
                    OnPropertyChanged();
                    UpdateBankAccountFields();
                }
            }
        }

        public CashAsset SelectedCashAsset
        {
            get => _selectedCashAsset;
            set
            {
                if (_selectedCashAsset != value)
                {
                    _selectedCashAsset = value;
                    OnPropertyChanged();
                    UpdateCashAssetFields();
                }
            }
        }

        // Bank Account Form Properties
        private string _bankName;
        public string BankName
        {
            get => _bankName;
            set
            {
                if (_bankName != value)
                {
                    _bankName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _accountHolder;
        public string AccountHolder
        {
            get => _accountHolder;
            set
            {
                if (_accountHolder != value)
                {
                    _accountHolder = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _accountType;
        public string AccountType
        {
            get => _accountType;
            set
            {
                if (_accountType != value)
                {
                    _accountType = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _accountNumber;
        public int AccountNumber
        {
            get => _accountNumber;
            set
            {
                if (_accountNumber != value)
                {
                    _accountNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _openingBalance;
        public double OpeningBalance
        {
            get => _openingBalance;
            set
            {
                if (_openingBalance != value)
                {
                    _openingBalance = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _swiftCode;
        public string SwiftCode
        {
            get => _swiftCode;
            set
            {
                if (_swiftCode != value)
                {
                    _swiftCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _branchLocation;
        public string BranchLocation
        {
            get => _branchLocation;
            set
            {
                if (_branchLocation != value)
                {
                    _branchLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _nominee;
        public string Nominee
        {
            get => _nominee;
            set
            {
                if (_nominee != value)
                {
                    _nominee = value;
                    OnPropertyChanged();
                }
            }
        }

        // Cash Asset Form Properties
        private string _cashLocation;
        public string CashLocation
        {
            get => _cashLocation;
            set
            {
                if (_cashLocation != value)
                {
                    _cashLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _currency;
        public string Currency
        {
            get => _currency;
            set
            {
                if (_currency != value)
                {
                    _currency = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _inCareOf;
        public string InCareOf
        {
            get => _inCareOf;
            set
            {
                if (_inCareOf != value)
                {
                    _inCareOf = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _amount;
        public double Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        public ICommand CreateAccountCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand SaveCashEntryCommand { get; }

        private bool CanExecuteCreateAccount(object parameter) => true;
        private bool CanExecuteSaveChanges(object parameter) => SelectedBankAccount != null;
        private bool CanExecuteSaveCashEntry(object parameter) => true;

        private async void ExecuteCreateAccount(object parameter)
        {
            var newAccount = new BankAccount
            {
                BankName = BankName,
                AccountHolder = AccountHolder,
                AccountType = AccountType,
                AccountNumber = AccountNumber,
                OpeningBalance = OpeningBalance,
                Country = Country,
                SwiftCode = SwiftCode,
                BranchLocation = BranchLocation,
                Nominee = Nominee
            };

            await _dataService.CreateBankAccountAsync(newAccount);
            BankAccounts.Add(newAccount);
            ClearBankAccountFields();
        }

        private async void ExecuteSaveChanges(object parameter)
        {
            if (SelectedBankAccount != null)
            {
                SelectedBankAccount.BankName = BankName;
                SelectedBankAccount.AccountHolder = AccountHolder;
                SelectedBankAccount.AccountType = AccountType;
                SelectedBankAccount.AccountNumber = AccountNumber;
                SelectedBankAccount.OpeningBalance = OpeningBalance;
                SelectedBankAccount.Country = Country;
                SelectedBankAccount.SwiftCode = SwiftCode;
                SelectedBankAccount.BranchLocation = BranchLocation;
                SelectedBankAccount.Nominee = Nominee;

                await _dataService.UpdateBankAccountAsync(SelectedBankAccount);
            }
        }

        private async void ExecuteSaveCashEntry(object parameter)
        {
            var newCashAsset = new CashAsset
            {
                Location = CashLocation,
                Currency = Currency,
                InCareOf = InCareOf,
                Amount = Amount
            };

            await _dataService.CreateCashAssetAsync(newCashAsset);
            CashAssets.Add(newCashAsset);
            ClearCashAssetFields();
        }

        #endregion

        #region Private Methods

        private async void LoadDataAsync()
        {
            try
            {
                var bankAccounts = await _dataService.GetBankAccountsAsync();
                var cashAssets = await _dataService.GetCashAssetsAsync();

                BankAccounts.Clear();
                CashAssets.Clear();

                foreach (var account in bankAccounts)
                {
                    BankAccounts.Add(account);
                }

                foreach (var asset in cashAssets)
                {
                    CashAssets.Add(asset);
                }
            }
            catch (Exception ex)
            {
                // Handle or log the error appropriately
                // You might want to implement IDialogService to show error messages
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        private void UpdateBankAccountFields()
        {
            if (SelectedBankAccount != null)
            {
                BankName = SelectedBankAccount.BankName;
                AccountHolder = SelectedBankAccount.AccountHolder;
                AccountType = SelectedBankAccount.AccountType;
                AccountNumber = SelectedBankAccount.AccountNumber;
                OpeningBalance = SelectedBankAccount.OpeningBalance;
                Country = SelectedBankAccount.Country;
                SwiftCode = SelectedBankAccount.SwiftCode;
                BranchLocation = SelectedBankAccount.BranchLocation;
                Nominee = SelectedBankAccount.Nominee;
            }
        }

        private void UpdateCashAssetFields()
        {
            if (SelectedCashAsset != null)
            {
                CashLocation = SelectedCashAsset.Location;
                Currency = SelectedCashAsset.Currency;
                InCareOf = SelectedCashAsset.InCareOf;
                Amount = SelectedCashAsset.Amount;
            }
        }

        private void ClearBankAccountFields()
        {
            BankName = string.Empty;
            AccountHolder = string.Empty;
            AccountType = string.Empty;
            AccountNumber = 0;
            OpeningBalance = 0;
            Country = string.Empty;
            SwiftCode = string.Empty;
            BranchLocation = string.Empty;
            Nominee = string.Empty;
        }

        private void ClearCashAssetFields()
        {
            CashLocation = string.Empty;
            Currency = string.Empty;
            InCareOf = string.Empty;
            Amount = 0;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    // Helper class for commands
    public class RelayCommand8 : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand8(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
    public interface IDataService
    {
        Task<IEnumerable<BankAccount>> GetBankAccountsAsync();
        Task<IEnumerable<CashAsset>> GetCashAssetsAsync();
        Task<BankAccount> CreateBankAccountAsync(BankAccount account);
        Task<BankAccount> UpdateBankAccountAsync(BankAccount account);
        Task<CashAsset> CreateCashAssetAsync(CashAsset asset);
        Task<CashAsset> UpdateCashAssetAsync(CashAsset asset);
    }

}
