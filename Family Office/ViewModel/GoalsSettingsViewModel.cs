using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Family_Office.Models;
using Family_Office.DataAccess;

namespace Family_Office.ViewModels
{
    public class GoalsSettingsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Goal> _goals;
        private Goal _selectedGoal;
        private readonly GoalDataAccess _dataAccess;

        public GoalsSettingsViewModel()
        {
            _dataAccess = new GoalDataAccess();
            Goals = new ObservableCollection<Goal>();
            LoadGoals();

            // Initialize Commands
            AddGoalCommand = new RelayCommand(ExecuteAddGoal);
            SaveChangesCommand = new RelayCommand(ExecuteSaveChanges);
            DeleteGoalCommand = new RelayCommand<int>(ExecuteDeleteGoal);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        #region Properties

        public ObservableCollection<Goal> Goals
        {
            get => _goals;
            set
            {
                _goals = value;
                OnPropertyChanged(nameof(Goals));
            }
        }

        public Goal SelectedGoal
        {
            get => _selectedGoal;
            set
            {
                _selectedGoal = value;
                OnPropertyChanged(nameof(SelectedGoal));
            }
        }

        #endregion

        #region Commands

        public ICommand AddGoalCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }
        public ICommand DeleteGoalCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        #endregion

        #region Command Implementations

        private void ExecuteAddGoal()
        {
            var newGoal = new Goal
            {
                VisionYear = DateTime.Now.Year,
                TargetAmount = 0,
                RateOfCompounding = 0
            };
            Goals.Add(newGoal);
            SelectedGoal = newGoal;
        }

        private void ExecuteSaveChanges()
        {
            try
            {
                foreach (var goal in Goals)
                {
                    if (goal.GoalID == 0)
                    {
                        GoalDataAccess.AddGoal(goal);
                    }
                    else
                    {
                        GoalDataAccess.UpdateGoal(goal);
                    }
                }
                LoadGoals(); // Refresh the list
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                // You might want to show a message to the user
                System.Diagnostics.Debug.WriteLine($"Error saving goals: {ex.Message}");
            }
        }

        private void ExecuteDeleteGoal(int goalId)
        {
            try
            {
                var goalToRemove = Goals.FirstOrDefault(g => g.GoalID == goalId);
                if (goalToRemove != null)
                {
                    GoalDataAccess.DeleteGoal(goalId);
                    Goals.Remove(goalToRemove);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                System.Diagnostics.Debug.WriteLine($"Error deleting goal: {ex.Message}");
            }
        }

        private void ExecuteCancel()
        {
            LoadGoals(); // Reload original data, discarding changes
        }

        #endregion

        #region Helper Methods

        private void LoadGoals()
        {
            try
            {
                var loadedGoals = GoalDataAccess.GetGoals();
                Goals.Clear();
                foreach (var goal in loadedGoals)
                {
                    Goals.Add(goal);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                System.Diagnostics.Debug.WriteLine($"Error loading goals: {ex.Message}");
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    // Helper class for commands
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
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }

    // Helper class for commands with parameters
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
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