using Family_Office.Models;

using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Family_Office.DataAccess;

namespace Family_Office.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private Settings _settings;

        public Settings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public SettingsViewModel()
        {
            Settings = SettingDataAccess.GetSettings();
        }

        public ICommand SaveCommand => new RelayCommand(SaveSettings);

        private void SaveSettings()
        {
            SettingDataAccess.UpdateSettings(Settings);
            MessageBox.Show("Settings have been saved!");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}