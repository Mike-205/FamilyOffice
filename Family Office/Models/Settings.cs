using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class Settings
    {
        public int settingsID { get; set; }
        public string DateFormat { get; set; } 
        public string BaseCurrency { get; set; }
        public bool ShowExchangeRatesInList { get; set; }
        public bool ShowBaseCurrencyEquivalent { get; set; }
        public bool UseThousandsSeparator { get; set; }
        public int DecimalPlaces { get; set; }
        public string FontFamily { get; set; }
        public int BodyFontSize { get; set; }
        public int HeaderFontSize { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string BackgroundColor { get; set; }
        public bool HighContrast { get; set; }
        public bool LargeTextMode { get; set; }
        public int Scale { get; set; }
        public int MarginSize { get; set; }
        public double BorderWidth { get; set; }
        public string BackgroundImage { get; set; } 
        public bool EnableHoverEffects { get; set; } 
        public bool ShowFocusIndication { get; set; }
        public bool UseAnimation { get; set; }
    }
}