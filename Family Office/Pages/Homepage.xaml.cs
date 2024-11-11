using Family_Office.DataAccess;
using Family_Office.Models;
using Family_Office.Views;
using Family_Office.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;

namespace Family_Office.pages
{
    public partial class Homepage : Page, INotifyPropertyChanged
    {
        private Settings _currentSettings;
        private readonly SettingsViewModel _settingsViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public Homepage()
        {
            InitializeComponent();
            _settingsViewModel = new SettingsViewModel();
            DataContext = this;

            LoadAndApplySettings();
            _settingsViewModel.PropertyChanged += SettingsViewModel_PropertyChanged;
            LoadDefaultPage();
        }

        private void LoadAndApplySettings()
        {
            try
            {
                Debug.WriteLine("Loading settings...");
                _currentSettings = SettingDataAccess.GetSettings();

                if (_currentSettings == null)
                {
                    Debug.WriteLine("No settings found, creating defaults...");
                    _currentSettings = new Settings
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
                }

                ApplySettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading settings: {ex.Message}");
                MessageBox.Show("Error loading settings. Default values will be used.",
                    "Settings Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ApplySettings()
        {
            try
            {
                var globalStyle = new Style(typeof(FrameworkElement));

                globalStyle.Setters.Add(new Setter(TextBlock.FontFamilyProperty,
                    new FontFamily(_currentSettings.FontFamily)));
                globalStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty,
                    _currentSettings.BodyFontSize));

                if (_currentSettings.HighContrast)
                {
                    Sidebar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                }
                else
                {
                    Sidebar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentSettings.PrimaryColor));
                    this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentSettings.BackgroundColor));
                }

                double scaleFactor = _currentSettings.Scale / 100.0;
                this.LayoutTransform = new ScaleTransform(scaleFactor, scaleFactor);

                var buttonStyle = new Style(typeof(Button),
                    (Style)Resources["SidebarButtonStyle"]);

                buttonStyle.Setters.Add(new Setter(Button.FontFamilyProperty,
                    new FontFamily(_currentSettings.FontFamily)));
                buttonStyle.Setters.Add(new Setter(Button.FontSizeProperty,
                    _currentSettings.BodyFontSize));
                buttonStyle.Setters.Add(new Setter(Button.MarginProperty,
                    new Thickness(_currentSettings.MarginSize)));
                buttonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty,
                    new Thickness(_currentSettings.BorderWidth)));

                if (_currentSettings.EnableHoverEffects)
                {
                    var trigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                    trigger.Setters.Add(new Setter(Button.BackgroundProperty,
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3f3f3f"))));
                    buttonStyle.Triggers.Add(trigger);
                }

                Resources["SidebarButtonStyle"] = buttonStyle;

                if (_currentSettings.LargeTextMode)
                {
                    globalStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty,
                        _currentSettings.BodyFontSize * 1.5));
                }

                if (!string.IsNullOrEmpty(_currentSettings.BackgroundImage))
                {
                    try
                    {
                        var imageBrush = new ImageBrush(new ImageSourceConverter()
                            .ConvertFromString(_currentSettings.BackgroundImage) as ImageSource);
                        MainContentFrame.Background = imageBrush;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading background image: {ex.Message}");
                    }
                }

                Resources.Add(typeof(FrameworkElement), globalStyle);

                InvalidateVisual();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error applying settings: {ex.Message}");
            }
        }

        private void SettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            LoadAndApplySettings();
        }

        private void LoadDefaultPage()
        {
            var defaultPage = new FamilyOfficeMastersPage();
            ApplySettingsToPage(defaultPage);
            MainContentFrame.Navigate(defaultPage);
        }

        private void ApplySettingsToPage(Page page)
        {
            //fetch settings again
            LoadAndApplySettings();
            if (page != null && _currentSettings != null)
            {
                if (_currentSettings.BodyFontSize == 0)
                {
                    return;
                }

                page.FontFamily = new FontFamily(_currentSettings.FontFamily);
                page.FontSize = _currentSettings.BodyFontSize;

                if (_currentSettings.LargeTextMode)
                {
                    page.FontSize = _currentSettings.BodyFontSize * 1.5;
                }

                double scaleFactor = _currentSettings.Scale / 100.0;
                page.LayoutTransform = new ScaleTransform(scaleFactor, scaleFactor);

                if (_currentSettings.HighContrast)
                {
                    page.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                    page.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
                }
                else
                {
                    page.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_currentSettings.BackgroundColor));
                }
            }
        }

        private void NavigateToSection(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var stackPanel = button.Content as StackPanel;
                var textBlock = stackPanel.Children[1] as TextBlock;
                string sectionName = textBlock.Text;

                Page targetPage = null;

                switch (sectionName)
                {
                    case "Family Office Masters":
                        targetPage = new FamilyOfficeMastersPage();
                        break;
                    case "Settings":
                        targetPage = new GeneralSettingsPage();
                        break;
                    case "Investments/Assets Master":
                        targetPage = new InvestmentView();
                        break;
                    case "Family Members Masters":
                        targetPage = new FamilyMembersView();
                        break;
                    case "Bank Management":
                        targetPage = new BankAccountPage();
                        break;
                    case "Third Party":
                        targetPage = new ThirdPartyView();
                        break;
                    case "Reports":
                        targetPage = new ReportsPage();
                        break;
                    case "Goal Settings":
                        targetPage = new GoalSettingsPage();
                        break;
                }

                if (targetPage != null)
                {
                    try
                    {
                        MainContentFrame.Navigate(targetPage);
                        ApplySettingsToPage(targetPage);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Navigation Error: {ex.Message}");
                        MessageBox.Show($"Error navigating to {sectionName}. Please try again.",
                            "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}