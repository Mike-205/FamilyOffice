using Family_Office.Models;
using Family_Office.DataAccess;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System.ComponentModel;
using Family_Office.Views;
using System.Data.SQLite;

namespace Family_Office.ViewModel
{
    public class ExchangeRateManagementViewModel : ViewModelBase
    {
        private ObservableCollection<CurrencyExchangeRate> _currencyExchangeRates;
        private ObservableCollection<Currency> _currencies;
        private Currency _selectedFromCurrency;
        private Currency _selectedToCurrency;
        private decimal _quickUpdateRate;
        private bool _isNewCurrencyExchangeRate;

        public ExchangeRateManagementViewModel()
        {
            LoadCurrencyTypes();
            LoadCurrencies();
            InitializeCommands();
        }

        private void LoadCurrencyTypes()
        {
            var rates = CurrencyExchangeRateDataAccess.GetExchangeRates();
            CurrencyExchangeRates = new ObservableCollection<CurrencyExchangeRate>(rates);
        }

        private void LoadCurrencies()
        {
            var currencies = CurrencyDataAccess.GetCurrencies();
            Currencies = new ObservableCollection<Currency>(currencies);
        }

        private void InitializeCommands()
        {
            AddCurrencyCommand = new RelayCommand(ExecuteAddCurrency);
            UpdateCurrencyExchangeRateCommand = new RelayCommand(ExecuteUpdateExchangeRate, CanExecuteUpdateExchangeRate);
        }

        public ObservableCollection<CurrencyExchangeRate> CurrencyExchangeRates
        {
            get => _currencyExchangeRates;
            set
            {
                _currencyExchangeRates = value;
                OnPropertyChanged(nameof(CurrencyExchangeRates));
            }
        }

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set
            {
                _currencies = value;
                OnPropertyChanged(nameof(Currencies));
            }
        }

        public Currency SelectedFromCurrency
        {
            get => _selectedFromCurrency;
            set
            {
                _selectedFromCurrency = value;
                OnPropertyChanged(nameof(SelectedFromCurrency));
                (UpdateCurrencyExchangeRateCommand as RelayCommand9)?.RaiseCanExecuteChanged();
            }
        }

        public Currency SelectedToCurrency
        {
            get => _selectedToCurrency;
            set
            {
                _selectedToCurrency = value;
                OnPropertyChanged(nameof(SelectedToCurrency));
                (UpdateCurrencyExchangeRateCommand as RelayCommand9)?.RaiseCanExecuteChanged();
            }
        }

        public decimal QuickUpdateRate
        {
            get => _quickUpdateRate;
            set
            {
                _quickUpdateRate = value;
                OnPropertyChanged(nameof(QuickUpdateRate));
                (UpdateCurrencyExchangeRateCommand as RelayCommand9)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddCurrencyCommand { get; private set; }
        public ICommand UpdateCurrencyExchangeRateCommand { get; private set; }

        private void ExecuteAddCurrency()
        {
            try
            {
                var addCurrencyWindow = new AddCurrencyForm();
                var viewModel = new AddCurrencyFormViewModel();

                viewModel.SaveCompleted += (s, e) =>
                {
                    LoadCurrencies();
                    LoadCurrencyTypes();
                    addCurrencyWindow.Close();
                };

                addCurrencyWindow.DataContext = viewModel;
                addCurrencyWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Add Currency window: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteUpdateExchangeRate()
        {
            return SelectedFromCurrency != null &&
                   SelectedToCurrency != null &&
                   SelectedFromCurrency != SelectedToCurrency &&
                   QuickUpdateRate > 0;
        }

        private void ExecuteUpdateExchangeRate()
        {
            var existingRate = CurrencyExchangeRates.FirstOrDefault(r =>
                r.FromCurrencyName == SelectedFromCurrency.CurrencyName &&
                r.ToCurrencyName == SelectedToCurrency.CurrencyName);

            if (existingRate == null)
            {
                var result = MessageBox.Show(
                    "This exchange rate pair does not exist. Would you like to create it?",
                    "New Exchange Rate",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var newRate = new CurrencyExchangeRate
                    {
                        FromCurrencyName = SelectedFromCurrency.CurrencyName,
                        ToCurrencyName = SelectedToCurrency.CurrencyName,
                        ExchangeRate = QuickUpdateRate
                    };

                    CurrencyExchangeRateDataAccess.AddExchangeRate(newRate);
                    LoadCurrencyTypes();
                }
            }
            else
            {
                existingRate.ExchangeRate = QuickUpdateRate;
                CurrencyExchangeRateDataAccess.UpdateExchangeRate(existingRate);
                MessageBox.Show("Exchange rate updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    public class AddCurrencyFormViewModel : ViewModelBase, IDataErrorInfo
    {
        private Currency _currency;
        private bool _isNewCurrencyType;
        private string _currencyCode;
        private string _currencyName;
        private string _currencySymbol;

        public AddCurrencyFormViewModel()
        {
            _currency = new Currency();
            _isNewCurrencyType = true;
            InitializeCommands();
        }

        public event EventHandler SaveCompleted;

        public string CurrencyCode
        {
            get => _currencyCode;
            set
            {
                _currencyCode = value?.ToUpper();
                OnPropertyChanged(nameof(CurrencyCode));
                (SaveCurrencyCommand as RelayCommand9)?.RaiseCanExecuteChanged();
            }
        }

        public string CurrencyName
        {
            get => _currencyName;
            set
            {
                _currencyName = value;
                OnPropertyChanged(nameof(CurrencyName));
                (SaveCurrencyCommand as RelayCommand9)?.RaiseCanExecuteChanged();
            }
        }

        public string CurrencySymbol
        {
            get => _currencySymbol;
            set
            {
                _currencySymbol = value;
                OnPropertyChanged(nameof(CurrencySymbol));
            }
        }

        public ICommand SaveCurrencyCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        private void InitializeCommands()
        {
            SaveCurrencyCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private bool CanExecuteSave()
        {
            return !string.IsNullOrWhiteSpace(CurrencyCode) &&
                   !string.IsNullOrWhiteSpace(CurrencyName) &&
                   CurrencyCode.Length == 3;
        }

        private void ExecuteSave()
        {
            var currency = new Currency
            {
                CurrencyName = CurrencyCode,
                CurrencySymbol = CurrencySymbol,
            };

            CurrencyDataAccess.AddCurrency(currency);
            MessageBox.Show("Currency added successfully!", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            SaveCompleted?.Invoke(this, EventArgs.Empty);
        }

        private void ExecuteCancel()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            window?.Close();
        }

        // IDataErrorInfo implementation
        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                string error = null;
                switch (propertyName)
                {
                    case nameof(CurrencyCode):
                        if (string.IsNullOrWhiteSpace(CurrencyCode))
                            error = "Currency Code is required";
                        else if (CurrencyCode.Length != 3)
                            error = "Currency Code must be exactly 3 characters";
                        break;

                    case nameof(CurrencyName):
                        if (string.IsNullOrWhiteSpace(CurrencyName))
                            error = "Currency Name is required";
                        break;
                }
                return error;
            }
        }
    }

    public class RelayCommand9 : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand9(Action execute, Func<bool> canExecute = null)
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

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}