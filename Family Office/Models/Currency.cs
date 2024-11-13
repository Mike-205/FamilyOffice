using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family_Office.Models
{
    public class Currency
    {
        [Key]
        public int CurrencyID { get; set; }

        [Required]
        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        // Navigation property for exchange rates where this currency is the "From" currency
        public virtual ICollection<CurrencyExchangeRate> FromExchangeRates { get; set; }

        // Navigation property for exchange rates where this currency is the "To" currency
        public virtual ICollection<CurrencyExchangeRate> ToExchangeRates { get; set; }

        public Currency()
        {
            FromExchangeRates = new HashSet<CurrencyExchangeRate>();
            ToExchangeRates = new HashSet<CurrencyExchangeRate>();
        }
    }
}
